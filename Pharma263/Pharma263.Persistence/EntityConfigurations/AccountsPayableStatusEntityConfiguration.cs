using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pharma263.Domain.Entities;
using Pharma263.Persistence.EntityConfigurations.Shared;
using System;
using System.Collections.Generic;

namespace Pharma263.Persistence.EntityConfigurations
{
    public class AccountsPayableStatusEntityConfiguration : ConcurrencyTokenEntityConfiguration<AccountsPayableStatus>, IEntityTypeConfiguration<AccountsPayableStatus>
    {
        public void Configure(EntityTypeBuilder<AccountsPayableStatus> builder)
        {
            builder.ToTable("AccountsPayableStatus", "Pharma263");
            builder.HasKey(b => b.Id);

            builder.Property(b => b.Name).HasColumnName("Name").HasMaxLength(50).IsRequired();
            builder.Property(b => b.Description).HasColumnName("Description").HasMaxLength(150).IsRequired(false);

            ConfigureConcurrencyToken(builder);

            //Indexes
            builder.HasIndex(b => b.Name, "UX_AccountsPayableStatus_Name").IsUnique();

            List<AccountsPayableStatus> seedData = new List<AccountsPayableStatus>(){
                new AccountsPayableStatus("Unpaid", "The account has not been paid yet"){
                    Id = 1,
                    CreatedBy = "System",
                    CreatedDate = new DateTimeOffset(2024, 05, 03, 08, 00, 00, TimeSpan.Zero),
                    IsDeleted = false
                },
                new AccountsPayableStatus("Partially Paid", "The account has been partially paid"){
                    Id = 2,
                    CreatedBy = "System",
                    CreatedDate = new DateTimeOffset(2024, 05, 03, 08, 00, 00, TimeSpan.Zero),
                    IsDeleted = false
                },
                new AccountsPayableStatus("Paid", "The account has been fully paid up"){
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
