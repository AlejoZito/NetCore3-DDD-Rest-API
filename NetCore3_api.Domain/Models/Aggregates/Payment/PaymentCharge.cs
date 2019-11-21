using NetCore3_api.Domain.Models.Aggregates.Event;
using System;
using System.Collections.Generic;
using System.Text;

namespace NetCore3_api.Domain.Models.Aggregates.Payment
{
    //Many to many relationship required entity
    public class PaymentCharge : Entity
    {
        public decimal Amount { get; set; }
        public Charge Charge { get; set; }
        public Payment Payment { get; set; }
    }
}
