using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Arquetipo.Infrastructure.Outbox;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Arquetipo.Infrastructure.Configurations;
internal sealed class OutboxMessageConfiguration: IEntityTypeConfiguration<OutboxMessage>
{
    public void Configure(EntityTypeBuilder<OutboxMessage> builder)
    {
        builder.ToTable("outbox_messages");
        builder.HasKey(outboxMessage => outboxMessage.Id);

        //postgres tiene un tipo de datos jsonb, para representar jsones. Si fuera sqlserver, tiene que ser un varchar o text
        builder.Property(OutboxMessage => OutboxMessage.Content)
        .HasColumnType("jsonb");

    }
}
