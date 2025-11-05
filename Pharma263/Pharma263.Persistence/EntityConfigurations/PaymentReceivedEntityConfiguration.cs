using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pharma263.Domain.Entities;
using Pharma263.Persistence.EntityConfigurations.Shared;

namespace Pharma263.Persistence.EntityConfigurations
{
    public class PaymentReceivedEntityConfiguration : ConcurrencyTokenEntityConfiguration<PaymentReceived>, IEntityTypeConfiguration<PaymentReceived>
    {
        public void Configure(EntityTypeBuilder<PaymentReceived> builder)
        {
            builder.ToTable("PaymentReceived", "Pharma263");
            builder.HasKey(p => p.Id);

            builder.Property(p => p.AmountReceived).HasColumnName("AmountReceived").HasColumnType("DECIMAL(14,4)").HasDefaultValue(0).IsRequired();
            builder.Property(p => p.PaymentDate).HasColumnName("PaymentDate").IsRequired();
            builder.Property(p => p.PaymentMethodId).HasColumnName("PaymentMethodId").IsRequired();
            builder.Property(p => p.AccountsReceivableId).HasColumnName("AccountsReceivableId").IsRequired();
            builder.Property(p => p.CustomerId).HasColumnName("CustomerId").IsRequired();

            ConfigureConcurrencyToken(builder);

            //One to Many

            builder
                .HasOne(o => o.PaymentMethod)
                .WithMany()
                .HasForeignKey(f => f.PaymentMethodId)
                .IsRequired(false)
                .HasConstraintName("FK_PaymentReceived_PaymentMethod_PaymentMethodId")
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .HasOne(o => o.AccountsReceivable)
                .WithMany()
                .HasForeignKey(f => f.AccountsReceivableId)
                .IsRequired(false)
                .HasConstraintName("FK_PaymentReceived_AccountsReceivable_AccountsReceivableId")
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .HasOne(o => o.Customer)
                .WithMany()
                .HasForeignKey(f => f.CustomerId)
                .IsRequired(false)
                .HasConstraintName("FK_PaymentReceived_Customer_CustomerId")
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
