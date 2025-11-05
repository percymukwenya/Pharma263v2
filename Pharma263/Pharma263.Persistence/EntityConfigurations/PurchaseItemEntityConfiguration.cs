using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pharma263.Domain.Entities;
using Pharma263.Persistence.EntityConfigurations.Shared;

namespace Pharma263.Persistence.EntityConfigurations
{
    public class PurchaseItemEntityConfiguration : ConcurrencyTokenEntityConfiguration<PurchaseItems>, IEntityTypeConfiguration<PurchaseItems>
    {
        public void Configure(EntityTypeBuilder<PurchaseItems> builder)
        {
            builder.ToTable("PurchaseItems", "Pharma263");
            builder.HasKey(p => p.Id);

            builder.Property(p => p.Price).HasColumnName("Price").HasColumnType("DECIMAL(14,2)");
            builder.Property(p => p.Quantity).HasColumnName("Quantity").IsRequired();
            builder.Property(p => p.Amount).HasColumnType("DECIMAL(14,2)").IsRequired();
            builder.Property(b => b.MedicineId).HasColumnName("MedicineId").IsRequired();
            builder.Property(b => b.BatchNo).HasColumnName("BatchNo").HasMaxLength(50).IsRequired();
            builder.Property(b => b.PurchaseId).HasColumnName("PurchaseId").IsRequired();

            ConfigureConcurrencyToken(builder);

            builder
             .HasOne(p => p.Medicine)
            .WithMany()
            .HasForeignKey(p => p.MedicineId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);

            builder
                .HasOne(p => p.Purchase)
            .WithMany(p => p.Items)
            .HasForeignKey(p => p.PurchaseId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
