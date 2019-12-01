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
                .ForMember(dest => dest.EventTypeName, options => options.MapFrom(x => x.Event.Type.Name))
                .ForMember(dest => dest.UserId, options => options.MapFrom(x => x.Event.User.Id))
                .ForMember(dest => dest.Amount, options => options.MapFrom(x=>x.Amount.Amount))
                .ForMember(dest => dest.Currency, options => options.MapFrom(x => x.Amount.Currency));

            CreateMap<Charge, GetChargeResponse>()
                .ForMember(dest => dest.Category, options => options.MapFrom(x => x.Event.Type.Category.Name))
                .ForMember(dest => dest.User, options => options.MapFrom(x => x.Event.User.Username))
                .ForMember(dest => dest.Amount, options => options.MapFrom(x => x.Amount.Amount))
                .ForMember(dest => dest.UnPaidAmount, options => options.MapFrom(x => x.GetUnPaidAmount().Amount))
                .ForMember(dest => dest.Currency, options => options.MapFrom(x => x.Amount.Currency))
                .ForMember(dest => dest.Payments, options => options.MapFrom(x => x.Payments));

            CreateMap<Payment, GetPaymentResponse>();
            CreateMap<PaymentCharge, GetPaymentChargeResponse>();                
        }
    }
}
