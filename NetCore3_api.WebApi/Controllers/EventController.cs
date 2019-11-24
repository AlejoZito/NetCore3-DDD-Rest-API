using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NetCore3_api.Domain.Contracts;
using NetCore3_api.Domain.Contracts.Exceptions;
using NetCore3_api.Domain.DomainServices;
using NetCore3_api.Domain.Models.Aggregates.Event;
using NetCore3_api.Domain.Models.Aggregates.User;
using NetCore3_api.WebApi.DTOs;

namespace NetCore3_api.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventController : ControllerBase
    {
        readonly IRepository<Charge> _chargeRepository;
        readonly IRepository<EventType> _eventTypeRepository;
        readonly IRepository<User> _userRepository;
        public EventController(
            IRepository<Charge> chargeRepository,
            IRepository<EventType> eventTypeRepository,
            IRepository<User> userRepository)
        {
            _chargeRepository = chargeRepository;
            _eventTypeRepository = eventTypeRepository;
            _userRepository = userRepository;
        }


        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> Create(
            CreateEventRequest createEventRequest)
        { 
            //Get charge domain service
            ChargeService chargeService = new ChargeService(
                _chargeRepository,
                _eventTypeRepository,
                _userRepository);

            try
            {
                //Create new charge and attach event to it
                Charge createdCharge = await chargeService.CreateChargeWithEvent(
                    createEventRequest.Amount,
                    createEventRequest.Currency,
                    createEventRequest.UserId,
                    createEventRequest.EventTypeName);

                //ToDo: ver como parametrizar la url de destino
                return CreatedAtAction("Charges/Get", new { id = createdCharge.Id }, createdCharge);
            }
            catch (InvalidEntityException ex)
            {
                return BadRequest((Charge)ex.Model);
            }
        }
    }
}