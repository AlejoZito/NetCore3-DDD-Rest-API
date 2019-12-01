using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetCore3_api.WebApi.DTOs
{
    public class GetPaymentChargeResponse
    {
        [JsonProperty("payed_amount")]
        public decimal Amount { get; set; }
        [JsonProperty("payment_source")]
        public GetPaymentResponse Payment { get; set; }
    }
}
