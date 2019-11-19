using NetCore3_api.Domain.Models.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace NetCore3_api.Domain.Models
{
    public class Payment : Entity
    {
        public Currency Currency { get; set; }
        public decimal Amount { get; set; }
        public User User { get; set; }
        public List<ChargePayment> Charges { get; set; }
        public bool IsValid { get; set; }
    }
}
