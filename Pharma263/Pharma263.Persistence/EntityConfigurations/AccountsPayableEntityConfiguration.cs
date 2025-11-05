using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pharma263.Domain.Entities;
using Pharma263.Persistence.EntityConfigurations.Shared;

namespace Pharma263.Persistence.EntityConfigurations
{
    public class AccountsPayableEntityConfiguration : ConcurrencyTokenEntityConfiguration<AccountsPayable>, IEntityTypeConfiguration<AccountsPayable>
    {
        public void Configure(EntityTypeBuilder<AccountsPayable> builder)
        {
            builder.ToTable("AccountsPayable", "Pharma263");
            builder.HasKey(p => p.Id);

            builder.Property(p => p.AmountOwed).HasColumnName("AmountOwed").HasColumnType("DECIMAL(14,4)").HasDefaultValue(0).IsRequired();
            builder.Property(p => p.DueDate).HasColumnName("DueDate").IsRequired();
            builder.Property(p => p.AmountPaid).HasColumnName("AmountPaid").HasColumnType("DECIMAL(14,4)").HasDefaultValue(0).IsRequired();
            builder.Property(p => p.BalanceOwed).HasColumnName("BalanceOwed").HasColumnType("DECIMAL(14,4)").HasDefaultValue(0).IsRequired();
            builder.Property(p => p.AccountsPayableStatusId).HasColumnName("AccountsPayableStatusId").IsRequired();
            builder.Property(p => p.SupplierId).HasColumnName("SupplierId").IsRequired();

            ConfigureConcurrencyToken(builder);

            //One to Many

            builder
                .HasOne(o => o.AccountsPayableStatus)
                .WithMany()
                .HasForeignKey(f => f.AccountsPayableStatusId)
                .IsRequired(false)
                .HasConstraintName("FK_AccountsPayable_AccountsPayableStatus_AccountsPayableStatusId")
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .HasOne(o => o.Supplier)
                .WithMany()
                .HasForeignKey(f => f.SupplierId)
                .IsRequired(false)
                .HasConstraintName("FK_AccountsPayable_Supplier_SupplierId")
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
