using NetCore3_api.Domain.Enumerations;
using NetCore3_api.Domain.Models.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;
using U = NetCore3_api.Domain.Models.Aggregates.User;

namespace NetCore3_api.Domain.Models.Aggregates.Event
{
    public class Event : Entity
    {
        public EventType Type { get; set; }
        public DateTime Date { get; set; }
        public U.User User { get; set; }
        public Charge Charge { get; set; }
    }
}
