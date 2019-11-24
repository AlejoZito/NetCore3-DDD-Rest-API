using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetCore3_api.Domain.Models.Aggregates.Event;
using System;
using System.Collections.Generic;
using System.Text;

namespace NetCore3_api.Infrastructure.EntityConfigurations
{
    class EventConfiguration : IEntityTypeConfiguration<Event>
    {
        public void Configure(EntityTypeBuilder<Event> builder)
        {
            builder.ToTable("Events");

            builder
                .HasOne(e => e.Type)
                .WithMany()
                .IsRequired(true)
                .OnDelete(DeleteBehavior.NoAction);

            builder
                .HasOne(e => e.User)
                .WithMany()
                .IsRequired(true)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Ignore(x => x.ValidationErrors);
        }
    }
}
