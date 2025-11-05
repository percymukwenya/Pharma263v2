using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pharma263.Domain.Entities;
using Pharma263.Persistence.EntityConfigurations.Shared;

namespace Pharma263.Persistence.EntityConfigurations
{
    public class PaymentMadeEntityConfiguration : ConcurrencyTokenEntityConfiguration<PaymentMade>, IEntityTypeConfiguration<PaymentMade>
    {
        public void Configure(EntityTypeBuilder<PaymentMade> builder)
        {
            builder.ToTable("PaymentMade", "Pharma263");
            builder.HasKey(p => p.Id);

            builder.Property(p => p.AmountPaid).HasColumnName("AmountPaid").HasColumnType("DECIMAL(14,4)").HasDefaultValue(0).IsRequired();
            builder.Property(p => p.PaymentDate).HasColumnName("PaymentDate").IsRequired();
            builder.Property(p => p.PaymentMethodId).HasColumnName("PaymentMethodId").IsRequired();
            builder.Property(p => p.AccountPayableId).HasColumnName("AccountPayableId").IsRequired();
            builder.Property(p => p.SupplierId).HasColumnName("SupplierId").IsRequired();

            ConfigureConcurrencyToken(builder);

            //One to Many

            builder
                .HasOne(o => o.PaymentMethod)
                .WithMany()
                .HasForeignKey(f => f.PaymentMethodId)
                .IsRequired(false)
                .HasConstraintName("FK_PaymentMade_PaymentMethod_PaymentMethodId")
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .HasOne(o => o.AccountPayable)
                .WithMany()
                .HasForeignKey(f => f.AccountPayableId)
                .IsRequired(false)
                .HasConstraintName("FK_PaymentMade_AccountPayable_AccountPayableId")
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .HasOne(o => o.Supplier)
                .WithMany()
                .HasForeignKey(f => f.SupplierId)
                .IsRequired(false)
                .HasConstraintName("FK_PaymentMade_Supplier_SupplierId")
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
