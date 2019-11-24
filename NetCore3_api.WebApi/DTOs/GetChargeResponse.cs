using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetCore3_api.WebApi.DTOs
{
    public class GetChargeResponse
    {
        public decimal Amount { get; set; }
        public string Currency { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public List<GetPaymentResponse> Payments { get; set; }        
        public string User { get; set; }
        public string Category { get; set; }
        //Dont map event
        //public Event Event { get; set; }
    }
}
