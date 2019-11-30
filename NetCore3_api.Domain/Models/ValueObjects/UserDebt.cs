using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetCore3_api.Domain.Models.ValueObjects
{
    public class UserDebt
    {
        private readonly List<AmountCurrency> DebtByCurrency;
        public UserDebt()
        {
            DebtByCurrency = new List<AmountCurrency>();
        }
        public void AddDebtAmount(AmountCurrency debtAmount)
        {
            //Check if this currency exists in the DebtByCurrency list
            var debtItem = DebtByCurrency.FirstOrDefault(x => x.Currency == debtAmount.Currency);
            if(debtItem != null)
            {
                debtItem.Amount += debtAmount.Amount;
            }
            else
            {
                //If no amount with this currency has been added yet,
                //create a new item and add this amount
                DebtByCurrency.Add(new AmountCurrency(debtAmount.Amount, debtAmount.Currency.Value));
            }
        }
        public List<AmountCurrency> GetDebtByCurrency()
        {
            return DebtByCurrency;
        }
    }
}
