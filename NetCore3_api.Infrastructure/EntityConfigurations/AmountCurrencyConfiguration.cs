//using Microsoft.EntityFrameworkCore;
//using Microsoft.EntityFrameworkCore.Metadata.Builders;
//using NetCore3_api.Domain.Models.Aggregates.Event;
//using NetCore3_api.Domain.Models.ValueObjects;
//using System;
//using System.Collections.Generic;
//using System.Text;

//namespace NetCore3_api.Infrastructure.EntityConfigurations
//{
//    class AmountCurrencyConfiguration : IEntityTypeConfiguration<AmountCurrency>
//    {
//        public void Configure(EntityTypeBuilder<AmountCurrency> builder)
//        {
//            //builder.ToTable("Charges");

//            //Amount value object persisted as owned entity in EF Core >2.0
//            //builder.OwnsOne(o => o.Amount);

//            builder.Ignore(x => x.ValidationErrors);
//        }
//    }
//}
