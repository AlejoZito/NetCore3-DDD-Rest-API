using NetCore3_api.Domain.Models.Aggregates.Payment;
using NetCore3_api.Domain.Models.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace NetCore3_api.Domain.Models.Aggregates.Event
{
    public class Charge : Entity
    {
        public decimal Amount { get; set; }
        public Currency Currency { get; set; }
        public ChargeCategory Category { get; set; }
        public List<PaymentCharge> Payments { get; set; }
    }
}
