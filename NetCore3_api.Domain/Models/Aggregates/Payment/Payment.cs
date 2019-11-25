using NetCore3_api.Domain.Enumerations;
using System;
using System.Collections.Generic;
using System.Text;
using U = NetCore3_api.Domain.Models.Aggregates.User;

namespace NetCore3_api.Domain.Models.Aggregates.Payment
{
    public class Payment : Entity
    {
        public Payment() { }
        public Payment(decimal amount, Currency currency, U.User user)
        {
            Amount = amount;
            Currency = currency;
            User = user;
        }

        public decimal Amount { get; set; }
        public Currency Currency { get; set; }
        public U.User User { get; set; }
        public DateTime Date {get;set;}
        public List<PaymentCharge> Charges { get; set; }
    }
}
