using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pharma263.Domain.Entities;
using Pharma263.Persistence.EntityConfigurations.Shared;

namespace Pharma263.Persistence.EntityConfigurations
{
    public class SalesItemEntityConfiguration : ConcurrencyTokenEntityConfiguration<SalesItems>, IEntityTypeConfiguration<SalesItems>
    {
        public void Configure(EntityTypeBuilder<SalesItems> builder)
        {
            builder.ToTable("SalesItems", "Pharma263");
            builder.HasKey(s => s.Id);

            builder.Property(i => i.Price).HasColumnName("Price").HasColumnType("DECIMAL(14,2)").IsRequired();
            builder.Property(i => i.Quantity).HasColumnName("Quantity").IsRequired();
            builder.Property(i => i.Amount).HasColumnName("Amount").HasColumnType("DECIMAL(14,2)").IsRequired();
            builder.Property(b => b.StockId).HasColumnName("StockId").IsRequired();

            ConfigureConcurrencyToken(builder);

            builder.HasOne(i => i.Stock)
                .WithMany()
                .HasForeignKey(i => i.StockId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(i => i.Sale)
                .WithMany(s => s.Items)
                .HasForeignKey(i => i.SaleId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
