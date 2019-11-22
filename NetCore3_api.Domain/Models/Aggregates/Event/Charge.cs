using NetCore3_api.Domain.Models.Aggregates.Payment;
using System;
using System.Collections.Generic;
using System.Text;

namespace NetCore3_api.Domain.Models.Aggregates.Event
{
    public class Charge : Entity
    {
        public decimal Amount { get; set; }
        public string Currency { get; set; }
        public List<PaymentCharge> Payments { get; set; }
        public Event Event { get; set; }
    }
}
