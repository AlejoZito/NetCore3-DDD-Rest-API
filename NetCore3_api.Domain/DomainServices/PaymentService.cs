using NetCore3_api.Domain.Contracts;
using NetCore3_api.Domain.Enumerations;
using NetCore3_api.Domain.Models.Aggregates.Event;
using NetCore3_api.Domain.Models.Aggregates.Payment;
using NetCore3_api.Domain.Models.Aggregates.User;
using NetCore3_api.Domain.Models.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace NetCore3_api.Domain.DomainServices
{
    class PaymentService
    {
        IRepository<Charge> _chargeRepository;
        IRepository<Payment> _paymentRepository;
        IRepository<User> _userRepository;
        public PaymentService(
            IRepository<Charge> chargeRepository,
            IRepository<Payment> paymentRepository,
            IRepository<User> userRepository)
        {
            _chargeRepository = chargeRepository;
            _paymentRepository = paymentRepository;
            _userRepository = userRepository;
        }
        public async Task<Payment> ExecutePayment(decimal amount, string currency, long userId)
        {
            var user = await _userRepository.FindByIdAsync(userId);

            if (!Enum.TryParse(typeof(Currency), currency, out object foundCurrency))
                throw new ArgumentException("Invalid currency");

            //Create Payment entity
            Payment payment = new Payment(amount, (Currency)foundCurrency, user);

            //1) Validate payment is valid with business rules (DOES NOT EXCEED USER DEBTH)
            UserDebtService userDebtService = new UserDebtService(_chargeRepository, _userRepository);
            AmountCurrency userDebtAmount = await userDebtService.GetUserDebt(userId, (Currency)foundCurrency);

            if (payment.Amount > userDebtAmount.Amount)
                throw new ArgumentException("Payment amount cannot exceed user debt in this currency");

            //2) Handle logic to find appropiate charges to pay
            var linkedCharges = await userDebtService.LinkPaymentToCharges(payment, userId);

            //3) Add linked charges to payment
            payment.Charges = linkedCharges;

            //Persist
            return _paymentRepository.Insert(payment);
        }
    }
}
