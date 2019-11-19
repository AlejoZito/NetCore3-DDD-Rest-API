using System;
using System.Collections.Generic;
using System.Text;

namespace NetCore3_api.Domain.Models
{
    public class Charge : Entity
    {
        public Event Event { get; set; }
        public User User { get; set; }
        public ChargeCategory Category { get; set; }
        public List<ChargePayment> Payments { get; set; }
    }
}
