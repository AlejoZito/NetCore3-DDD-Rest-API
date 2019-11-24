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
    [Route("api/charges")]
    [ApiController]
    public class ChargeController : ControllerBase
    {
        readonly IRepository<Charge> _chargeRepository;
        readonly IRepository<EventType> _eventTypeRepository;
        readonly IRepository<User> _userRepository;
        readonly IMapper _mapper;
        readonly AppDbContext _dbContext;
        public ChargeController(
            IRepository<Charge> chargeRepository,
            IRepository<EventType> eventTypeRepository,
            IRepository<User> userRepository,
            IMapper mapper,
            AppDbContext dbContext)
        {
            _chargeRepository = chargeRepository;
            _eventTypeRepository = eventTypeRepository;
            _userRepository = userRepository;
            _mapper = mapper;
            _dbContext = dbContext;
        }


        // GET: api/charges
        [HttpGet]
        [Produces(typeof(GetChargeResponse))]
        public async Task<ActionResult> Get(int? pageSize = null, int? pageNumber = null)
        {
            //ToDo implement pagination
            return Ok(_mapper.Map<List<GetChargeResponse>>(await _chargeRepository.ListAsync(
                    sortOptions: new SortOptions(nameof(Charge.Id), SortOrder.Descending),
                    pageSize: pageSize,
                    pageNumber: pageNumber)));
        }

        // GET: api/charges/5
        [HttpGet("{id}")]
        [Produces(typeof(List<GetChargeResponse>))]
        public async Task<ActionResult> Get(int id)
        {
            //ToDo falta referenciar las entidades hijas
            var moduleProfile = await _chargeRepository.FindByIdAsync(id);

            if (moduleProfile != null)
                return Ok(_mapper.Map<GetChargeResponse>(moduleProfile));
            else
                return NotFound($"No module profile was found with id {id}");
        }
    }
}