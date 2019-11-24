using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetCore3_api.WebApi.DTOs
{
    public class CreateEventRequest
    {
        public decimal Amount { get; internal set; }
        public string Currency { get; internal set; }
        public long UserId { get; internal set; }
        public string EventTypeName { get; internal set; }
    }
}
