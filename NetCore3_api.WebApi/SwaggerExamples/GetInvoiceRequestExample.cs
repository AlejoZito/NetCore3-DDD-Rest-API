using NetCore3_api.WebApi.DTOs;
using Swashbuckle.AspNetCore.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetCore3_api.WebApi.SwaggerExamples
{
    public class GetInvoicesRequestExample : IExamplesProvider<GetInvoicesRequest>
    {
        GetInvoicesRequest IExamplesProvider<GetInvoicesRequest>.GetExamples()
        {
            return new GetInvoicesRequest
            {
                From = new MonthYearDTO() { Year = 2019, Month = 11},
                To = new MonthYearDTO() { Year = 2020, Month = 02 }
            };
        }
    }
}
