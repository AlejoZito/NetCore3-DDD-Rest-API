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
using NetCore3_api.Domain.Models.Aggregates.Payment;
using NetCore3_api.Domain.Models.Aggregates.User;
using NetCore3_api.Infrastructure;
using NetCore3_api.WebApi.DTOs;
using NetCore3_api.WebApi.SwaggerExamples;
using Swashbuckle.AspNetCore.Filters;

namespace NetCore3_api.WebApi.Controllers
{
    [Route("api")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        readonly IRepository<Payment> _paymentRepository;
        readonly IRepository<Charge> _chargeRepository;
        readonly IRepository<User> _userRepository;
        readonly IRepository<Invoice> _invoiceRepository;
        readonly IMapper _mapper;
        readonly AppDbContext _dbContext;
        public PaymentController(
            IRepository<Payment> paymentRepository,
            IRepository<Charge> chargeRepository,
            IRepository<User> userRepository,
            IRepository<Invoice> invoiceRepository,
            IMapper mapper,
            AppDbContext dbContext)
        {
            _paymentRepository = paymentRepository;
            _chargeRepository = chargeRepository;
            _userRepository = userRepository;
            _invoiceRepository = invoiceRepository;
            _mapper = mapper;
            _dbContext = dbContext;
        }

        /// <summary>
        /// If the payment is valid (e.g. doesn't exceed current user's debt) a new payment is created, linked to the corresponding 
        /// unpaid charges and associated to an invoice. To select unpaid charges to fulfil, the oldest charges are selected first.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="createPaymentRequest">The amount must be > 0 and the currency valid (ARS, US)</param>
        /// <returns></returns>
        [HttpPost("users/{userId}/payments")]
        [Produces(typeof(Charge))]        
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> Create(
            [FromRoute]long userId,
            [FromBody]CreatePaymentRequest createPaymentRequest)
        {
            UserDebtService userDebtService = new UserDebtService(_chargeRepository);
            InvoiceService invoiceService = new InvoiceService(_invoiceRepository);

            //Get charge domain service
            PaymentService paymentService = new PaymentService(
                _paymentRepository,
                _userRepository,
                userDebtService,
                invoiceService);

            try
            {
                Payment createdPayment = 
                    await paymentService.ExecutePayment(createPaymentRequest.Amount, createPaymentRequest.Currency, userId);

                await _dbContext.SaveChangesAsync();

                return Ok(_mapper.Map<GetPaymentResponse>(createdPayment));
            }
            catch (InvalidEntityException ex)
            {
                return BadRequest(ex.Model);
            }
        }

        // GET: api/payments
        /// <summary>
        /// Get all user payments. Optional parameters allow result paging and sorting, default sort order is descending and will show most recent payments first.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageNumber"></param>
        /// <param name="sortOrder"></param>
        /// <returns></returns>
        [HttpGet("users/{userId}/payments")]
        [Produces(typeof(GetPaymentResponse))]
        public async Task<ActionResult> Get(long userId, int? pageSize = null, int? pageNumber = null, string sortOrder = null)
        {
            SortOrder parsedSortOrder = SortOrder.Descending; //Default sort order shows most recent items first

            if (!string.IsNullOrEmpty(sortOrder))
            {
                //If sort order is not null, check it has a valid value
                if (!SortOrderHelper.TryParse(sortOrder, out parsedSortOrder))
                    return BadRequest($"Sort order '' is not valid, enter ASC or DESC");
            }

            var payments = await _paymentRepository.ListAsync(
                    predicate: x=>x.User.Id == userId,
                    sortOptions: new SortOptions(nameof(Charge.Id), parsedSortOrder),
                    pageSize: pageSize,
                    pageNumber: pageNumber);

            return Ok(_mapper.Map<List<GetPaymentResponse>>(payments));
        }

        // GET: api/payments/5
        /// <summary>
        /// Get a specific payment for a user
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="id">Charge Id</param>
        /// <returns></returns>
        [HttpGet("users/{userId}/payments/{id}")]
        [Produces(typeof(GetPaymentResponse))]
        public async Task<ActionResult> Get(long userId, long id)
        {
            var payment = await _paymentRepository.FindAsync(x=>x.Id == id && x.User.Id == userId);
            
            if (payment != null)
                return Ok(_mapper.Map<GetPaymentResponse>(payment));
            else
                return NotFound($"No payment was found for id {id}");
        }
    }
}