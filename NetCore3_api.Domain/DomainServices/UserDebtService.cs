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

        public UserDebtService(
            IRepository<Charge> chargeRepository)
        {
            _chargeRepository = chargeRepository;
        }

        /// <summary>
        /// Get the total amount of user debt grouped by currency
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<UserDebt> GetUserDebt(long userId)
        {
            var unPaidCharges = await _chargeRepository.ListAsync(x =>
                x.Event.User.Id == userId &&
                x.Payments.Sum(x => x.Amount) < x.Amount.Amount);

            var userDebt = new UserDebt();

            //Store debt separated by currency in value object
            unPaidCharges.ForEach(c => userDebt.AddDebtAmount(c.GetUnPaidAmount()));

            return userDebt;
        }

        /// <summary>
        /// Gets the user's debt in a specific currency
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="currency"></param>
        /// <returns></returns>
        public async Task<AmountCurrency> GetUserDebtByCurrency(long userId, Currency currency)
        {
            //Get all charges that have not been fully paid, for this currency type
            var unPaidCharges = await _chargeRepository.ListAsync(x =>
                x.Event.User.Id == userId &&
                x.Amount.Currency == currency &&
                x.Payments.Sum(x => x.Amount) < x.Amount.Amount);

            if (unPaidCharges != null && unPaidCharges.Count > 0)
                //Sum the unpaid amount of all unpaid charges
                return new AmountCurrency(unPaidCharges.Sum(x => x.GetUnPaidAmount().Amount), currency);
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
        public async Task<bool> IsValidPayment(Payment payment)
        {
            AmountCurrency userDebtAmount = await GetUserDebtByCurrency(payment.User.Id, payment.Currency);
            return payment.Amount <= userDebtAmount.Amount;
        }
    }
}
