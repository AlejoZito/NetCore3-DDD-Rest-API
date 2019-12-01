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
    public class DebtController : ControllerBase
    {
        readonly IRepository<Charge> _chargeRepository;
        readonly IRepository<EventType> _eventTypeRepository;
        readonly IRepository<User> _userRepository;
        readonly IMapper _mapper;
        readonly AppDbContext _dbContext;
        public DebtController(
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


        // GET: api/users/5/debts
        /// <summary>
        /// Get the user's debt (the sum of all unpaid charges). If the user has debt in different currencies, they will be shown as different debt resources
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpGet("users/{userId}/debts")]
        [Produces(typeof(GetUserDebtResponse))]
        public async Task<ActionResult> Get(long userId)
        {
            UserDebtService userDebtService = new UserDebtService(_chargeRepository);
            var userDebt = await userDebtService.GetUserDebt(userId);

            if (userDebt != null && userDebt.GetDebtAmountsByCurrency().Count > 0)
                return Ok(_mapper.Map<GetUserDebtResponse>(userDebt));
            else
                return Ok("User has no debt");
        }
    }
}