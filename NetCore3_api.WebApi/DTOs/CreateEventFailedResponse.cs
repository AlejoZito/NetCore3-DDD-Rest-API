using NetCore3_api.Domain.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetCore3_api.WebApi.DTOs
{
    public class CreateEventFailedResponse
    {
        public decimal Amount { get; set; }
        public string Currency { get; set; }
        public long UserId { get; set; }
        public string EventTypeName { get; set; }
        public List<ValidationError> ValidationErrors { get; set; }
    }
}
