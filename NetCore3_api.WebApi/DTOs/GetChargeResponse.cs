using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetCore3_api.WebApi.DTOs
{
    public class GetChargeResponse
    {
        public long Id { get; set; }
        public decimal Amount { get; set; }
        [JsonProperty("unpaid_amount")]
        public decimal UnPaidAmount { get; set; }
        public string Currency { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public List<GetPaymentChargeResponse> Payments { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string User { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Category { get; set; }

        public DateTime Date { get; set; }
        //Dont map event
        //public Event Event { get; set; }
    }
}
