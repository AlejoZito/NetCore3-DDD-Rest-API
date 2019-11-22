using NetCore3_api.Domain.Contracts;
using NetCore3_api.Domain.Enumerations;
using NetCore3_api.Domain.Models.Aggregates.Event;
using NetCore3_api.Domain.Models.Aggregates.User;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace NetCore3_api.Domain.DomainServices
{
    public class ChargeService
    {
        readonly IRepository<Charge> _chargeRepository;
        readonly IRepository<EventType> _eventTypeRepository;
        readonly IRepository<User> _userRepository;

        public async Task<Charge> CreateEvent(
            decimal amount,
            string currency,
            long userId,
            string eventTypeName)
        {
            //Build charge entity
            Charge charge = new Charge()
            {
                Amount = amount,
                Currency = currency.ToLower(),
                Event = new Event()
                {
                    //Build event entity and fetch references
                    //(will be used to validate valid eventTypeName and userId)
                    Type = await _eventTypeRepository.FindAsync(x => x.Name.ToLower() == eventTypeName.ToLower()),
                    Date = DateTime.Now,
                    User = await _userRepository.FindByIdAsync(userId),
                }
            };

            //ToDo:
            //if(charge.IsValid())
            return _chargeRepository.Insert(charge);

            //Ver si hay un invoice creado para este mes/año del user
            //o crear uno nuevo


            //Incrementar deuda del user
        }
    }
}
