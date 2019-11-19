using NetCore3_api.Domain.Enumerations;
using System;
using System.Collections.Generic;
using System.Text;

namespace NetCore3_api.Domain.Models
{
    public class ChargeCategory : Entity
    {
        public string Name { get; set; }
        public List<EventType> AssociatedEventTypes { get; set; }
    }
}
