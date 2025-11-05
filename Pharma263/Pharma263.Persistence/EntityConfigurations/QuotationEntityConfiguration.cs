using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pharma263.Domain.Entities;
using Pharma263.Persistence.EntityConfigurations.Shared;

namespace Pharma263.Persistence.EntityConfigurations
{
    public class QuotationEntityConfiguration : ConcurrencyTokenEntityConfiguration<Quotation>, IEntityTypeConfiguration<Quotation>
    {
        public void Configure(EntityTypeBuilder<Quotation> builder)
        {
            builder.ToTable("Quotation", "Pharma263");
            builder.HasKey(p => p.Id);

            builder.Property(b => b.QuotationDate).HasColumnName("QuotationDate").IsRequired();
            builder.Property(b => b.Notes).HasColumnName("Notes").HasMaxLength(200).IsRequired(false);
            builder.Property(b => b.Total).HasColumnName("Total").HasColumnType("DECIMAL(14,2)").IsRequired();
            builder.Property(b => b.Discount).HasColumnName("Discount").HasColumnType("DECIMAL(14,2)").IsRequired();
            builder.Property(b => b.GrandTotal).HasColumnName("GrandTotal").HasColumnType("DECIMAL(14,2)").IsRequired();
            builder.Property(b => b.QuoteExpiryDate).HasColumnName("QuoteExpiryDate").IsRequired(false);

            builder.Property(b => b.QuoteStatusId).HasColumnName("QuoteStatusId").IsRequired();
            builder.Property(b => b.CustomerId).HasColumnName("CustomerId").IsRequired();

            ConfigureConcurrencyToken(builder);

            //One To Many
            builder
                .HasOne(c => c.QuoteStatus)
                .WithMany()
                .HasForeignKey(c => c.QuoteStatusId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .HasOne(c => c.Customer)
                .WithMany()
                .HasForeignKey(c => c.CustomerId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
