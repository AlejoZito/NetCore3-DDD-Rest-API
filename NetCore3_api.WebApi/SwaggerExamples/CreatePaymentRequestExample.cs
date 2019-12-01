using NetCore3_api.WebApi.DTOs;
using Swashbuckle.AspNetCore.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetCore3_api.WebApi.SwaggerExamples
{
    public class CreatePaymentRequestExample : IExamplesProvider<CreatePaymentRequest>
    {
        CreatePaymentRequest IExamplesProvider<CreatePaymentRequest>.GetExamples()
        {

            return new CreatePaymentRequest
            {
                Amount = 100,
                Currency = "ARS",
            };
        }
    }
}
