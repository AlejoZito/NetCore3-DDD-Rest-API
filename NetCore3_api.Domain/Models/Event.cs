using NetCore3_api.Domain.Enumerations;
using NetCore3_api.Domain.Models.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace NetCore3_api.Domain.Models
{
    public class Event : Entity
    {
        public decimal Amount { get; set; }
        public Currency Currency { get; set; }
        public User User { get; set; }
        public EventType Type { get; set; }
        public DateTime Date { get; set; }
    }
}
