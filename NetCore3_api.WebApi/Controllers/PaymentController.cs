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

        [HttpPost("users/{userId}/payments")]
        [Produces(typeof(Charge))]
        //[Produces(typeof(CreateEventFailedResponse))]
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
        [HttpGet("users/{userId}/payments")]
        [Produces(typeof(GetPaymentResponse))]
        public async Task<ActionResult> Get(long userId, int? pageSize = null, int? pageNumber = null)
        {
            //ToDo implement pagination
            var payments = await _paymentRepository.ListAsync(
                    predicate: x=>x.User.Id == userId,
                    sortOptions: new SortOptions(nameof(Charge.Id), SortOrder.Descending),
                    pageSize: pageSize,
                    pageNumber: pageNumber);

            return Ok(_mapper.Map<List<GetPaymentResponse>>(payments));
        }

        // GET: api/payments/5
        [HttpGet("users/{userId}/payments/{id}")]
        [Produces(typeof(GetPaymentResponse))]
        public async Task<ActionResult> Get(long userId, long id)
        {
            //ToDo falta referenciar las entidades hijas
            var payment = await _paymentRepository.FindAsync(x=>x.Id == id && x.User.Id == userId);
            
            if (payment != null)
                return Ok(_mapper.Map<GetPaymentResponse>(payment));
            else
                return NotFound($"No payment was found for id {id}");
        }
    }
}