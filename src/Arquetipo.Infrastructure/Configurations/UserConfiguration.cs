using Arquetipo.Domain.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Arquetipo.Infrastructure.Configurations;

internal sealed class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
          builder.ToTable("users");
        builder.HasKey(user => user.Id);

        //builder.HasMany()
        // all user table configuration: relations, constraints, conversions HERE 👇👇
    }
}

