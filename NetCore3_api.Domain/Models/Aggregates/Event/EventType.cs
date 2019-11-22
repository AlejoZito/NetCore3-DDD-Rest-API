using System;
using System.Collections.Generic;
using System.Text;

namespace NetCore3_api.Domain.Models.Aggregates.Event
{
    public class EventType : Entity
    {
        public string Name { get; set; }
        public ChargeCategory Category{ get; set; }
    }
}
