using NetCore3_api.Domain.Models.ValueObjects;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetCore3_api.WebApi.DTOs
{
    public class GetInvoicesResponse
    {
        public List<GetInvoiceResponse> Invoices { get; set; }
    }
    public class GetInvoiceResponse
    {
        public int Year { get; set; }
        public int Month { get; set; }
        public List<GetChargeResponse> Charges { get; set; }
        public List<GetPaymentResponse> Payments { get; set; }
    }
}
