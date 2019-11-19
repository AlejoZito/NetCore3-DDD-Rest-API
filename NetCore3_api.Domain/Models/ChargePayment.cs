using System;
using System.Collections.Generic;
using System.Text;

namespace NetCore3_api.Domain.Models
{
    //Relationship between payment and charges
    public class ChargePayment : Entity
    {
        public decimal Amount { get; set; }
        public Charge Charge { get; set; }
        public Payment Payment { get; set; }
    }
}
