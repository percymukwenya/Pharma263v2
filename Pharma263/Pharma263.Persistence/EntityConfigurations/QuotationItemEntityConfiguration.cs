using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pharma263.Domain.Entities;
using Pharma263.Persistence.EntityConfigurations.Shared;

namespace Pharma263.Persistence.EntityConfigurations
{
    public class QuotationItemEntityConfiguration : ConcurrencyTokenEntityConfiguration<QuotationItems>, IEntityTypeConfiguration<QuotationItems>
    {
        public void Configure(EntityTypeBuilder<QuotationItems> builder)
        {
            builder.ToTable("QuotationItems", "Pharma263");
            builder.HasKey(p => p.Id);

            builder.Property(i => i.Price).HasColumnName("Price").HasColumnType("DECIMAL(14,2)").IsRequired();
            builder.Property(i => i.Quantity).HasColumnName("Quantity").IsRequired();
            builder.Property(i => i.Amount).HasColumnName("Amount").HasColumnType("DECIMAL(14,2)").IsRequired();
            builder.Property(b => b.StockId).HasColumnName("StockId").IsRequired();

            ConfigureConcurrencyToken(builder);

            builder
             .HasOne(p => p.Stock)
            .WithMany()
            .HasForeignKey(p => p.StockId)
            .OnDelete(DeleteBehavior.Restrict);

            builder
             .HasOne(p => p.Quotation)
            .WithMany(p => p.Items)
            .HasForeignKey(p => p.QuotationId)
            .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
