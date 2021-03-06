﻿using NetCore3_api.Domain.Contracts;
using NetCore3_api.Domain.Contracts.Exceptions;
using NetCore3_api.Domain.Enumerations;
using NetCore3_api.Domain.Models.Aggregates.Event;
using NetCore3_api.Domain.Models.Aggregates.User;
using NetCore3_api.Domain.Models.ValueObjects;
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
        readonly IInvoiceService _invoiceService;
        public ChargeService(
            IRepository<Charge> chargeRepository,
            IRepository<EventType> eventTypeRepository,
            IRepository<User> userRepository,
            IInvoiceService invoiceService)
        {
            _chargeRepository = chargeRepository;
            _eventTypeRepository = eventTypeRepository;
            _userRepository = userRepository;
            _invoiceService = invoiceService;
        }
        public async Task<Charge> CreateChargeWithEvent(
            decimal amount,
            string currency,
            long userId,
            string eventTypeName)
        {
            if (string.IsNullOrEmpty(eventTypeName))
                eventTypeName = "";

            var user = await _userRepository.FindByIdAsync(userId);

            //Build charge entity
            Charge charge = new Charge()
            {
                Amount = new AmountCurrency(amount, currency?.ToUpper()),
                Event = new Event()
                {
                    //Build event entity and fetch references
                    //(will be used to validate valid eventTypeName and userId)
                    Type = await _eventTypeRepository.FindAsync(x => x.Name.ToLower() == eventTypeName.ToLower()),
                    Date = DateTime.Now,
                    User = user,
                }
            };

            if (charge.IsValid())
            {
                //Add charge to existing invoice or create a new one
                await _invoiceService.AddCharge(charge, user);

                return _chargeRepository.Insert(charge);
            }
            else
                throw new InvalidEntityException(charge);
        }
    }
}
