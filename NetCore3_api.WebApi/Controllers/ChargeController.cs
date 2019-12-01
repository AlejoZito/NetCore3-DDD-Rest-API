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
        /// <summary>
        /// Returns charges for a specific user. Optional parameters allow result paging and sorting, default sort order is descending and will show most recent charges first.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageNumber">First page = 1, not 0</param>
        /// <param name="sortOrder">ASC, DESC</param>
        /// <returns></returns>
        [HttpGet("users/{userId}/charges")]
        [Produces(typeof(GetChargeResponse))]
        public async Task<ActionResult> Get(long userId, int? pageSize = null, int? pageNumber = null, string sortOrder = null)
        {
            SortOrder parsedSortOrder = SortOrder.Descending; //Default sort order shows most recent items first

            if (!string.IsNullOrEmpty(sortOrder))
            {
                //If sort order is not null, check it has a valid value
                if (!SortOrderHelper.TryParse(sortOrder, out parsedSortOrder))
                    return BadRequest($"Sort order '' is not valid, enter ASC or DESC");
            }

            var charges = await _chargeRepository.ListAsync(
                    predicate: x => x.Event.User.Id == userId,
                    sortOptions: new SortOptions(nameof(Charge.Id), parsedSortOrder),
                    pageSize: pageSize,
                    pageNumber: pageNumber);

            return Ok(_mapper.Map<List<GetChargeResponse>>(charges));
        }

        // GET: api/charges/5
        [HttpGet("users/{userId}/charges/{id}")]
        [Produces(typeof(GetChargeResponse))]
        public async Task<ActionResult> Get(long userId, long id)
        {
            var charge = await _chargeRepository.FindAsync(x => x.Event.User.Id == userId && x.Id == id);

            if (charge != null)
                return Ok(_mapper.Map<GetChargeResponse>(charge));
            else
                return NotFound($"No module profile was found with id {id}");
        }
    }
}