using NetCore3_api.Domain.Enumerations;
using NetCore3_api.Domain.Models.Aggregates.Payment;
using NetCore3_api.Domain.Models.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace NetCore3_api.Domain.Contracts
{
    public interface IUserDebtService
    {
        Task<AmountCurrency> GetUserDebtByCurrency(long userId, Currency currency);
        Task<bool> IsValidPayment(Payment payment);
    }
}
