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
                FromMonth = 11,
                FromYear = 2019,
                ToMonth = 2,
                ToYear = 2020,
            };
        }
    }
}
