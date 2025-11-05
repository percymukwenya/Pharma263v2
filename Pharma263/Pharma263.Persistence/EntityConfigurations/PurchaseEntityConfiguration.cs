using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pharma263.Domain.Entities;
using Pharma263.Persistence.EntityConfigurations.Shared;

namespace Pharma263.Persistence.EntityConfigurations
{
    public class PurchaseEntityConfiguration : ConcurrencyTokenEntityConfiguration<Purchase>, IEntityTypeConfiguration<Purchase>
    {
        public void Configure(EntityTypeBuilder<Purchase> builder)
        {
            builder.ToTable("Purchase", "Pharma263");
            builder.HasKey(b => b.Id);

            builder.Property(b => b.PurchaseDate).HasColumnName("PurchaseDate").IsRequired();
            builder.Property(b => b.Notes).HasColumnName("Notes").HasMaxLength(200).IsRequired(false);
            builder.Property(b => b.Total).HasColumnName("Total").HasColumnType("DECIMAL(14,2)").IsRequired();            
            builder.Property(b => b.Discount).HasColumnName("Discount").HasColumnType("DECIMAL(14,2)").IsRequired();
            builder.Property(b => b.GrandTotal).HasColumnName("GrandTotal").HasColumnType("DECIMAL(14,2)").IsRequired();
            builder.Property(b => b.PaymentDueDate).HasColumnName("PaymentDueDate").IsRequired(false);

            builder.Property(b => b.SupplierId).HasColumnName("SupplierId").IsRequired();
            builder.Property(b => b.PaymentMethodId).HasColumnName("PaymentMethodId").IsRequired();
            builder.Property(b => b.PurchaseStatusId).HasColumnName("PurchaseStatusId").IsRequired();

            ConfigureConcurrencyToken(builder);

            builder.HasIndex(b => new { b.Id, b.PurchaseDate }, "IX_Purchase_Id_PurchaseDate").IsUnique();

            builder
                .HasOne(b => b.Supplier)
                .WithMany()
                .HasForeignKey(b => b.SupplierId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .HasOne(b => b.PaymentMethod)
                .WithMany()
                .HasForeignKey(b => b.PaymentMethodId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .HasOne(b => b.PurchaseStatus)
                .WithMany()
                .HasForeignKey(b => b.PurchaseStatusId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
