using System;
using System.Collections.Generic;
using System.Text;

namespace NetCore3_api.Domain.Models
{
    public class Invoice : Entity
    {
        public int Month { get; set; }
        public int Year { get; set; }
        public User User { get; set; }
        public List<Charge> Charges { get; set; }
    }
}
