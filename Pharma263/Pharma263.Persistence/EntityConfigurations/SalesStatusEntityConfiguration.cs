using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pharma263.Domain.Entities;
using Pharma263.Persistence.EntityConfigurations.Shared;
using System.Collections.Generic;
using System;

namespace Pharma263.Persistence.EntityConfigurations
{
    public class SalesStatusEntityConfiguration : ConcurrencyTokenEntityConfiguration<SaleStatus>, IEntityTypeConfiguration<SaleStatus>
    {
        public void Configure(EntityTypeBuilder<SaleStatus> builder)
        {
            builder.ToTable("SaleStatus", "Pharma263");
            builder.HasKey(b => b.Id);

            builder.Property(b => b.Name).HasColumnName("Name").HasMaxLength(50).IsRequired();
            builder.Property(b => b.Description).HasColumnName("Description").HasMaxLength(150).IsRequired(false);

            ConfigureConcurrencyToken(builder);

            //Indexes
            builder.HasIndex(b => b.Name, "UX_SaleStatus_Name").IsUnique();

            List<SaleStatus> seedData = new List<SaleStatus>(){
                new SaleStatus("Due", "Due date manually added"){
                    Id = 1,
                    CreatedBy = "System",
                    CreatedDate = new DateTimeOffset(2024, 05, 03, 08, 00, 00, TimeSpan.Zero),
                    IsDeleted = false
                },
                new SaleStatus("Partially Paid", "Purchase partially paid"){
                    Id = 2,
                    CreatedBy = "System",
                    CreatedDate = new DateTimeOffset(2024, 05, 03, 08, 00, 00, TimeSpan.Zero),
                    IsDeleted = false
                },
                new SaleStatus("Fully Paid", "Fully Paid"){
                    Id = 3,
                    CreatedBy = "System",
                    CreatedDate = new DateTimeOffset(2024, 05, 03, 08, 00, 00, TimeSpan.Zero),
                    IsDeleted = false
                },
                new SaleStatus("Due - 7 days", "Payment due in 7 days"){
                    Id = 4,
                    CreatedBy = "System",
                    CreatedDate = new DateTimeOffset(2024, 05, 03, 08, 00, 00, TimeSpan.Zero),
                    IsDeleted = false
                },
                new SaleStatus("Due - 14 days", "Payment due in 14 days"){
                    Id = 5,
                    CreatedBy = "System",
                    CreatedDate = new DateTimeOffset(2024, 05, 03, 08, 00, 00, TimeSpan.Zero),
                    IsDeleted = false
                },
                new SaleStatus("Due - 30 days", "Payment due in 30 days"){
                    Id = 6,
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
