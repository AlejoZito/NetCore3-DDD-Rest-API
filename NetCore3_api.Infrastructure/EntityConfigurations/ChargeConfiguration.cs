﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetCore3_api.Domain.Models.Aggregates.Event;
using System;
using System.Collections.Generic;
using System.Text;

namespace NetCore3_api.Infrastructure.EntityConfigurations
{
    class ChargeConfiguration : IEntityTypeConfiguration<Charge>
    {
        public void Configure(EntityTypeBuilder<Charge> builder)
        {
            builder.ToTable("Charges");

            //Amount value object persisted in same table
            builder.OwnsOne(o => o.Amount).Ignore(x=>x.ValidationErrors);

            builder.Ignore(x => x.ValidationErrors);
        }
    }
}
