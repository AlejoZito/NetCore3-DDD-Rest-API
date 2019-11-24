using AutoMapper;
using NetCore3_api.Domain.Models.Aggregates.Event;
using NetCore3_api.Domain.Models.Aggregates.Payment;
using NetCore3_api.WebApi.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetCore3_api.WebApi.Mapping
{
    public class AutoMapping : Profile
    {
        public AutoMapping()
        {
            CreateMap<Charge, CreateEventFailedResponse>()
                .ForMember(dest => dest.EventTypeName, src => src.MapFrom(x => x.Event.Type.Name))
                .ForMember(dest => dest.UserId, src => src.MapFrom(x => x.Event.User.Id))
                .ForMember(dest => dest.Amount, src => src.MapFrom(x=>x.Amount.Amount))
                .ForMember(dest => dest.Currency, src => src.MapFrom(x => x.Amount.Currency));

            CreateMap<Charge, GetChargeResponse>()
                .ForMember(dest => dest.Category, src => src.MapFrom(x => x.Event.Type.Category.Name))
                .ForMember(dest => dest.User, src => src.MapFrom(x => x.Event.User.Username))
                .ForMember(dest => dest.Amount, src => src.MapFrom(x => x.Amount.Amount))
                .ForMember(dest => dest.Currency, src => src.MapFrom(x => x.Amount.Currency))
                .ForMember(dest => dest.Payments, src => src.MapFrom(x => x.Payments.Select(p=> p.Payment).ToList()));

            CreateMap<Payment, GetPaymentResponse>();
        }
    }
}
