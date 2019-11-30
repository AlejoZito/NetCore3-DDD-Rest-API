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
    public class UserDebtService : IUserDebtService
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
        /// Validate payment is valid for the user's debt.
        /// e.g.: Payment cannot exceed user's debt
        /// </summary>
        /// <param name="payment"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<bool> IsValidPayment(Payment payment, long userId)
        {
            AmountCurrency userDebtAmount = await GetUserDebt(userId, payment.Currency);
            return payment.Amount > userDebtAmount.Amount;
        }
    }
}
