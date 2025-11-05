using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pharma263.Domain.Entities;
using Pharma263.Persistence.EntityConfigurations.Shared;

namespace Pharma263.Persistence.EntityConfigurations
{
    public class AccountsReceivableEntityConfiguration : ConcurrencyTokenEntityConfiguration<AccountsReceivable>, IEntityTypeConfiguration<AccountsReceivable>
    {
        public void Configure(EntityTypeBuilder<AccountsReceivable> builder)
        {
            builder.ToTable("AccountsReceivable", "Pharma263");
            builder.HasKey(p => p.Id);

            builder.Property(p => p.AmountDue).HasColumnName("AmountDue").HasColumnType("DECIMAL(14,4)").HasDefaultValue(0).IsRequired();
            builder.Property(p => p.DueDate).HasColumnName("DueDate").IsRequired();
            builder.Property(p => p.AmountPaid).HasColumnName("AmountPaid").HasColumnType("DECIMAL(14,4)").HasDefaultValue(0).IsRequired();
            builder.Property(p => p.BalanceDue).HasColumnName("BalanceDue").HasColumnType("DECIMAL(14,4)").HasDefaultValue(0).IsRequired();
            builder.Property(p => p.AccountsReceivableStatusId).HasColumnName("AccountsReceivableStatusId").IsRequired();
            builder.Property(p => p.CustomerId).HasColumnName("CustomerId").IsRequired();

            ConfigureConcurrencyToken(builder);

            //One to Many

            builder
                .HasOne(o => o.AccountsReceivableStatus)
                .WithMany()
                .HasForeignKey(f => f.AccountsReceivableStatusId)
                .IsRequired(false)
                .HasConstraintName("FK_AccountsReceivable_AccountsReceivableStatus_AccountsReceivableStatusId")
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .HasOne(o => o.Customer)
                .WithMany()
                .HasForeignKey(f => f.CustomerId)
                .IsRequired(false)
                .HasConstraintName("FK_AccountsPayable_Customer_CustomerId")
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
