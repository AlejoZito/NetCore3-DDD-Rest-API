using System;
using System.Collections.Generic;
using System.Text;
using U = NetCore3_api.Domain.Models.Aggregates.User;

namespace NetCore3_api.Domain.Models.Aggregates.Payment
{
    public class Payment : Entity
    {
        public string Currency { get; set; }
        public decimal Amount { get; set; }
        public U.User User { get; set; }
        public List<PaymentCharge> Charges { get; set; }
    }
}
