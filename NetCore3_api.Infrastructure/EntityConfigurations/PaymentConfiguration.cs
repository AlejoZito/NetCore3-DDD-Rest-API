using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetCore3_api.Domain.Models.Aggregates.Event;
using NetCore3_api.Domain.Models.Aggregates.Payment;
using System;
using System.Collections.Generic;
using System.Text;

namespace NetCore3_api.Infrastructure.EntityConfigurations
{
    class PaymentConfiguration : IEntityTypeConfiguration<Payment>
    {
        public void Configure(EntityTypeBuilder<Payment> builder)
        {
            builder.ToTable("Payments");

            builder.HasMany(x => x.Charges)
                .WithOne(x => x.Payment)
                .IsRequired();

            builder.Ignore(x => x.ValidationErrors);
        }
    }
}
