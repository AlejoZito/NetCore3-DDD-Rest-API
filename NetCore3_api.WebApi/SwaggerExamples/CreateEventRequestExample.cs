using NetCore3_api.WebApi.DTOs;
using Swashbuckle.AspNetCore.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetCore3_api.WebApi.SwaggerExamples
{
    public class CreateEventRequestExample : IExamplesProvider<CreateEventRequest>
    {
        CreateEventRequest IExamplesProvider<CreateEventRequest>.GetExamples()
        {

            return new CreateEventRequest
            {
                Amount = 100,
                Currency = "ARS",
                UserId = 1,
                EventTypeName = "MercadoPago"
            };
        }
    }
}
