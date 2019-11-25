using NetCore3_api.Domain.Contracts;
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
    public class UserDebtService
    {
        IRepository<Charge> _chargeRepository;
        IRepository<User> _userRepository;
        public UserDebtService(
            IRepository<Charge> chargeRepository,
            IRepository<User> userRepository)
        {
            _chargeRepository = chargeRepository;
            _userRepository = userRepository;
        }
        public async Task<AmountCurrency> GetUserDebt(long userId, Currency currency)
        {
            //Get all charges that have not been fully paid, for this currency type
            var unPaidCharges = await _chargeRepository.ListAsync(x =>
                x.Event.User.Id == userId &&
                x.Amount.Currency == currency &&
                x.Payments.Sum(x => x.Amount) < x.Amount.Amount);

            if (unPaidCharges != null && unPaidCharges.Count > 0)
                return new AmountCurrency(unPaidCharges.Sum(x => x.GetUnPaidAmount()), currency);
            else
                return new AmountCurrency(0, currency);
        }

        /// <summary>
        /// Find available charges to pay for this user, with the same currency the payment has.
        /// Assign a PaymentCharge entity to every charge that this payment can pay off.
        /// </summary>
        /// <param name="payment"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<List<PaymentCharge>> LinkPaymentToCharges(Payment payment, long userId)
        {
            List<PaymentCharge> generatedPaymentCharges = new List<PaymentCharge>();
            //Get all charges that have not been fully paid, for this currency type
            var unPaidCharges = await _chargeRepository.ListAsync(x =>
                x.Event.User.Id == userId &&
                x.Amount.Currency == payment.Currency &&
                x.Payments.Sum(x => x.Amount) < x.Amount.Amount);

            decimal paymentAmount = payment.Amount;
            int i = 0;
            while(paymentAmount > 0 && i < unPaidCharges.Count)
            {
                //decimal payedAmount = paymentAmount - unPaidCharges[i].Amount.Amount;

                if(paymentAmount - unPaidCharges[i].Amount.Amount > 0)
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
