using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetCore3_api.WebApi.DTOs
{
    public class CreateEventRequest
    {
        [JsonProperty("amount")]
        public decimal Amount { get; internal set; }
        [JsonProperty("currency")]
        public string Currency { get; internal set; }
        [JsonProperty("userId")]
        public long UserId { get; internal set; }
        [JsonProperty("eventTypeName")]
        public string EventTypeName { get; internal set; }
    }
}
