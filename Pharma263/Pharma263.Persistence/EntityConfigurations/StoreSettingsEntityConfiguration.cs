using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pharma263.Domain.Entities;
using Pharma263.Persistence.EntityConfigurations.Shared;
using System;
using System.Collections.Generic;

namespace Pharma263.Persistence.EntityConfigurations
{
    public class StoreSettingsEntityConfiguration : ConcurrencyTokenEntityConfiguration<StoreSetting>, IEntityTypeConfiguration<StoreSetting>
    {
        public void Configure(EntityTypeBuilder<StoreSetting> builder)
        {

            builder.ToTable("StoreSetting", "Pharma263");
            builder.HasKey(b => b.Id);
            builder.Property(b => b.Logo).HasColumnName("Logo").IsRequired(false);
            builder.Property(b => b.StoreName).HasColumnName("StoreName").HasMaxLength(150).IsRequired();
            builder.Property(b => b.Email).HasColumnName("Email").HasMaxLength(150).IsRequired();
            builder.Property(b => b.Phone).HasColumnName("Phone").HasMaxLength(25).IsRequired();
            builder.Property(b => b.Currency).HasColumnName("Currency").HasMaxLength(20).IsRequired();
            builder.Property(b => b.Address).HasColumnName("Address").HasMaxLength(255).IsRequired();
            builder.Property(b => b.MCAZLicence).HasColumnName("MCAZLicence").HasMaxLength(100).IsRequired(false);
            builder.Property(b => b.VATNumber).HasColumnName("VATNumber").HasMaxLength(100).IsRequired(false);
            builder.Property(b => b.BankingDetails).HasColumnName("BankingDetails").HasMaxLength(150).IsRequired(false);

            ConfigureConcurrencyToken(builder);
        }
    }
}
