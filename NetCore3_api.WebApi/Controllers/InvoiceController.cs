using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NetCore3_api.Domain.Contracts;
using NetCore3_api.Domain.Contracts.Exceptions;
using NetCore3_api.Domain.DomainServices;
using NetCore3_api.Domain.Models.Aggregates.Event;
using NetCore3_api.Domain.Models.Aggregates.User;
using NetCore3_api.Infrastructure;
using NetCore3_api.WebApi.DTOs;
using NetCore3_api.WebApi.SwaggerExamples;
using Swashbuckle.AspNetCore.Filters;

namespace NetCore3_api.WebApi.Controllers
{
    [Route("api/")]
    [ApiController]
    public class InvoiceController : ControllerBase
    {
        readonly IRepository<Invoice> _invoiceRepository;
        readonly IMapper _mapper;
        readonly AppDbContext _dbContext;
        public InvoiceController(
            IRepository<Invoice> invoiceRepository,
            IMapper mapper,
            AppDbContext dbContext)
        {
            _invoiceRepository = invoiceRepository;
            _mapper = mapper;
            _dbContext = dbContext;
        }


        // GET: api/users/5/invoices
        /// <summary>
        /// Get all invoices for a user. The request allows filtering by FROM and TO periods (month-year) or by a specific period.
        /// If a user has charges in 2 currencies, 2 invoices were created. Invoices are unique per user, period and currency. 
        /// (Maybe they can be joined in the frontend or refactored to store them as a single entity)
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="getInvoiceRequest">
        ///     Allows period filtering. IMPORTANT! Must specify both month and year to make the period parameter valid.
        ///     If, for example, From Month = 1 but From Year = null, the From period will be ignored.
        ///     If the SpecificMonthYear parameter was completed, 'from' and 'to' periods will be ignored.
        /// </param>
        /// <returns></returns>
        [HttpGet("users/{userId}/invoices")]
        [Produces(typeof(GetInvoicesResponse))]
        public async Task<ActionResult> Get([FromRoute]long userId, [FromQuery] GetInvoicesRequest getInvoiceRequest = null)
        {
            InvoiceService invoiceService = new InvoiceService(_invoiceRepository);

            //Tuple stores <FROM, TO> periods
            Tuple<MonthYearDTO, MonthYearDTO> periodToSearch = getInvoiceRequest.GetPeriodToSearch();

            var invoices = await invoiceService.GetInvoicesForUser(
                userId,
                periodToSearch.Item1?.Year, 
                periodToSearch.Item1?.Month, 
                periodToSearch.Item2?.Year,
                periodToSearch.Item2?.Month);

            if (invoices != null && invoices.Count > 0)
                return Ok(_mapper.Map<GetInvoicesResponse>(invoices));
            else
                return Ok("User has no invoices");
        }
    }
}