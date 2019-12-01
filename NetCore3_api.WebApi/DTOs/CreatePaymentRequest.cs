using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetCore3_api.WebApi.DTOs
{
    public class CreatePaymentRequest
    {
        [JsonProperty("amount")]
        public decimal Amount { get; internal set; }
        [JsonProperty("currency")]
        public string Currency { get; internal set; }
    }
}
