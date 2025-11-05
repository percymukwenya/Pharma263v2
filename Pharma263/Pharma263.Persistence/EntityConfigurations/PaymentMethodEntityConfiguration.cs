using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pharma263.Domain.Entities;
using Pharma263.Persistence.EntityConfigurations.Shared;
using System.Collections.Generic;
using System;

namespace Pharma263.Persistence.EntityConfigurations
{
    public class PaymentMethodEntityConfiguration : ConcurrencyTokenEntityConfiguration<PaymentMethod>, IEntityTypeConfiguration<PaymentMethod>
    {
        public void Configure(EntityTypeBuilder<PaymentMethod> builder)
        {
            builder.ToTable("PaymentMethod", "Pharma263");
            builder.HasKey(b => b.Id);

            builder.Property(b => b.Name).HasColumnName("Name").HasMaxLength(50).IsRequired();
            builder.Property(b => b.Description).HasColumnName("Description").HasMaxLength(150).IsRequired(false);

            ConfigureConcurrencyToken(builder);

            //Indexes
            builder.HasIndex(b => b.Name, "UX_PaymentMethod_Name").IsUnique();

            List<PaymentMethod> seedData = new List<PaymentMethod>(){
                new PaymentMethod("Cash - USD", "Cash payment in USD"){
                    Id = 1,
                    CreatedBy = "System",
                    CreatedDate = new DateTimeOffset(2024, 05, 03, 08, 00, 00, TimeSpan.Zero),
                    IsDeleted = false
                },
                new PaymentMethod("Cash - ZiG", "Cash payment in ZiG"){
                    Id = 2,
                    CreatedBy = "System",
                    CreatedDate = new DateTimeOffset(2024, 05, 03, 08, 00, 00, TimeSpan.Zero),
                    IsDeleted = false
                },
                new PaymentMethod("Mobile money", "Mobile money"){
                    Id = 3,
                    CreatedBy = "System",
                    CreatedDate = new DateTimeOffset(2024, 05, 03, 08, 00, 00, TimeSpan.Zero),
                    IsDeleted = false
                }
            };

            //Seed Data
            builder.HasData(seedData);
        }
    }
}
