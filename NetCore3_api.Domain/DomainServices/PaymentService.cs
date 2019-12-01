using NetCore3_api.Domain.Contracts;
using NetCore3_api.Domain.Contracts.Exceptions;
using NetCore3_api.Domain.Enumerations;
using NetCore3_api.Domain.Models.Aggregates.Event;
using NetCore3_api.Domain.Models.Aggregates.Payment;
using NetCore3_api.Domain.Models.Aggregates.User;
using NetCore3_api.Domain.Models.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetCore3_api.Domain.DomainServices
{
    public class PaymentService
    {
        IRepository<Payment> _paymentRepository;
        IRepository<User> _userRepository;
        IUserDebtService _userDebtService;
        IInvoiceService _invoiceService;
        public PaymentService(
            IRepository<Payment> paymentRepository,
            IRepository<User> userRepository,
            IUserDebtService userDebtService,
            IInvoiceService invoiceService)
        {
            _invoiceService = invoiceService;
            _paymentRepository = paymentRepository;
            _userRepository = userRepository;
            _userDebtService = userDebtService;
        }
        public async Task<Payment> ExecutePayment(decimal amount, string currency, long userId)
        {
            var user = await _userRepository.FindByIdAsync(userId);

            if (!Enum.TryParse(typeof(Currency), currency, out object foundCurrency))
                throw new ArgumentException("Invalid currency");

            //Create Payment entity
            Payment payment = new Payment(amount, (Currency)foundCurrency, user);
            if (payment.IsValid())
            {
                //1) Validate payment is valid with business rules (DOES NOT EXCEED USER DEBT)
                if (await _userDebtService.IsValidPayment(payment) == false)
                {
                    payment.ValidationErrors.Add(new ValidationError(nameof(Payment.Amount), "Payment amount cannot exceed current user's debt"));
                    throw new InvalidEntityException(payment);
                }

                //2) Find unpaid charges to pay
                var linkedCharges = await LinkPaymentToCharges(payment, userId);

                //3) Add linked charges to payment
                payment.Charges = linkedCharges;

                //4) Add payment to invoice
                await _invoiceService.AddPayment(payment, user);

                //Persist
                return _paymentRepository.Insert(payment);
            }
            else
                throw new InvalidEntityException(payment);
        }

        /// <summary>
        /// Find available charges to pay for this user, with the same currency the payment has.
        /// Assign a PaymentCharge entity to every charge that this payment can pay off.
        /// </summary>
        /// <param name="payment"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        private async Task<List<PaymentCharge>> LinkPaymentToCharges(Payment payment, long userId)
        {
            List<PaymentCharge> generatedPaymentCharges = new List<PaymentCharge>();
            //Get all charges that have not been fully paid, for this currency type
            //orders result by id, so that oldest charges will be paid first.
            var unPaidCharges = await _userDebtService.GetUnpaidCharges(userId, payment.Currency);

            //Variable that stores amount left on payment entity.
            //When linked to a charge, the charge's amount is substracted.
            decimal paymentAmountLeft = payment.Amount;
            int i = 0;
            while (paymentAmountLeft > 0 && i < unPaidCharges.Count)
            {
                decimal chargeUnPaidAmount = unPaidCharges[i].GetUnPaidAmount().Amount;

                //Check if payment's $ left can pay the charges full debt
                if (paymentAmountLeft - chargeUnPaidAmount >= 0)
                {
                    //The charge is payed completely
                    generatedPaymentCharges.Add(new PaymentCharge(unPaidCharges[i], payment, chargeUnPaidAmount));
                    paymentAmountLeft -= chargeUnPaidAmount; //Substract the amount of the remaining charge debt
                }
                else
                {
                    //The charge is partially paid
                    generatedPaymentCharges.Add(new PaymentCharge(unPaidCharges[i], payment, paymentAmountLeft));
                    //This payment cannot payoff more charges
                    paymentAmountLeft = 0;
                }
                i++;
            }

            return generatedPaymentCharges;
        }
    }
}
