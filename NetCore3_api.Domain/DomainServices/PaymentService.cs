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
    class PaymentService
    {
        IRepository<Charge> _chargeRepository;
        IRepository<Payment> _paymentRepository;
        IRepository<User> _userRepository;
        IUserDebtService _userDebtService;
        public PaymentService(
            IRepository<Charge> chargeRepository,
            IRepository<Payment> paymentRepository,
            IRepository<User> userRepository,
            IUserDebtService userDebtService)
        {
            _chargeRepository = chargeRepository;
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
                //1) Validate payment is valid with business rules (DOES NOT EXCEED USER DEBTH)
                if (await _userDebtService.IsValidPayment(payment, user.Id) == false)
                    throw new ArgumentException("Payment amount cannot exceed user debt in this currency");

                //2) Handle logic to find appropiate charges to pay
                var linkedCharges = await LinkPaymentToCharges(payment, userId);

                //3) Add linked charges to payment
                payment.Charges = linkedCharges;

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
            var unPaidCharges = await _chargeRepository.ListAsync(x =>
                x.Event.User.Id == userId &&
                x.Amount.Currency == payment.Currency &&
                x.Payments.Sum(x => x.Amount) < x.Amount.Amount);

            decimal paymentAmount = payment.Amount;
            int i = 0;
            while (paymentAmount > 0 && i < unPaidCharges.Count)
            {
                //decimal payedAmount = paymentAmount - unPaidCharges[i].Amount.Amount;

                if (paymentAmount - unPaidCharges[i].Amount.Amount > 0)
                {
                    //Payment pays off the charge completely
                    generatedPaymentCharges.Add(new PaymentCharge(unPaidCharges[i], payment, unPaidCharges[i].Amount.Amount));
                    paymentAmount -= unPaidCharges[i].Amount.Amount;
                }
                else
                {
                    //The charge is partially paid
                    generatedPaymentCharges.Add(new PaymentCharge(unPaidCharges[i], payment, paymentAmount));
                    //This payment cannot payoff more charges
                    paymentAmount = 0;
                }
            }

            return generatedPaymentCharges;
        }
    }
}
