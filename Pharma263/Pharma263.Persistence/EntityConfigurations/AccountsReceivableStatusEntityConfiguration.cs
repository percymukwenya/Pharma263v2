using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pharma263.Domain.Entities;
using Pharma263.Persistence.EntityConfigurations.Shared;
using System.Collections.Generic;
using System;

namespace Pharma263.Persistence.EntityConfigurations
{
    public class AccountsReceivableStatusEntityConfiguration : ConcurrencyTokenEntityConfiguration<AccountsReceivableStatus>, IEntityTypeConfiguration<AccountsReceivableStatus>
    {
        public void Configure(EntityTypeBuilder<AccountsReceivableStatus> builder)
        {
            builder.ToTable("AccountsReceivableStatus", "Pharma263");
            builder.HasKey(b => b.Id);

            builder.Property(b => b.Name).HasColumnName("Name").HasMaxLength(50).IsRequired();
            builder.Property(b => b.Description).HasColumnName("Description").HasMaxLength(150).IsRequired(false);

            ConfigureConcurrencyToken(builder);

            //Indexes
            builder.HasIndex(b => b.Name, "UX_AccountsReceivableStatus_Name").IsUnique();

            List<AccountsReceivableStatus> seedData = new List<AccountsReceivableStatus>(){
                new AccountsReceivableStatus("Unpaid", "The account has not been paid yet"){
                    Id = 1,
                    CreatedBy = "System",
                    CreatedDate = new DateTimeOffset(2024, 05, 03, 08, 00, 00, TimeSpan.Zero),
                    IsDeleted = false
                },
                new AccountsReceivableStatus("Partially Paid", "The account has been partially paid"){
                    Id = 2,
                    CreatedBy = "System",
                    CreatedDate = new DateTimeOffset(2024, 05, 03, 08, 00, 00, TimeSpan.Zero),
                    IsDeleted = false
                },
                new AccountsReceivableStatus("Paid", "The account has been fully paid up"){
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
