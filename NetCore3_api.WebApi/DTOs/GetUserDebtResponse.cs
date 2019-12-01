using NetCore3_api.Domain.Models.ValueObjects;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetCore3_api.WebApi.DTOs
{
    public class GetUserDebtResponse
    {
        public List<AmountCurrencyResponse> DebtAmounts { get; set; }
    }
}
