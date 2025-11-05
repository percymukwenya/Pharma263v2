using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pharma263.Domain.Entities;
using Pharma263.Persistence.EntityConfigurations.Shared;

namespace Pharma263.Persistence.EntityConfigurations
{
    public class StockEntityConfiguration : ConcurrencyTokenEntityConfiguration<Stock>, IEntityTypeConfiguration<Stock>
    {
        public void Configure(EntityTypeBuilder<Stock> builder)
        {
            builder.ToTable("Stock", "Pharma263");
            builder.HasKey(b => b.Id);

            builder.Property(b => b.TotalQuantity).HasColumnName("TotalQuantity").IsRequired();
            builder.Property(b => b.NotifyForQuantityBelow).HasColumnName("NotifyForQuantityBelow").IsRequired();
            builder.Property(b => b.MedicineId).HasColumnName("MedicineId").IsRequired();
            builder.Property(b => b.BatchNo).HasColumnName("BatchNo").HasMaxLength(50).IsRequired();
            builder.Property(b => b.ExpiryDate).HasColumnName("ExpiryDate").IsRequired();
            builder.Property(b => b.BuyingPrice).HasColumnName("BuyingPrice").HasColumnType("DECIMAL(14,2)").IsRequired();
            builder.Property(b => b.SellingPrice).HasColumnName("SellingPrice").HasColumnType("DECIMAL(14,2)").IsRequired();

            ConfigureConcurrencyToken(builder);

            builder.HasIndex(b => new { b.MedicineId, b.BatchNo }, "IX_Medicine_Name_BatchNo").IsUnique();

            builder
                .HasOne(x => x.Medicine)
                .WithMany()
                .HasForeignKey(x => x.MedicineId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
