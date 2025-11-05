using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pharma263.Domain.Entities;
using Pharma263.Persistence.EntityConfigurations.Shared;

namespace Pharma263.Persistence.EntityConfigurations
{
    public class ReturnEntityConfiguration : ConcurrencyTokenEntityConfiguration<Returns>, IEntityTypeConfiguration<Returns>
    {
        public void Configure(EntityTypeBuilder<Returns> builder)
        {
            builder.ToTable("Returns", "Pharma263");
            builder.HasKey(b => b.Id);

            builder.Property(b => b.Quantity).HasColumnName("Quantity").IsRequired();
            builder.Property(b => b.DateReturned).HasColumnName("DateReturned").IsRequired();

            builder.Property(b => b.ReturnReasonId).HasColumnName("ReturnReasonId").IsRequired();
            builder.Property(b => b.ReturnDestinationId).HasColumnName("ReturnDestinationId").IsRequired();
            builder.Property(b => b.ReturnStatusId).HasColumnName("ReturnStatusId").IsRequired();            
            builder.Property(b => b.StockId).HasColumnName("StockId").IsRequired();
            builder.Property(b => b.SaleId).HasColumnName("SaleId").IsRequired();

            ConfigureConcurrencyToken(builder);

            builder
                .HasOne(x => x.Sale)
                .WithMany()
                .HasForeignKey(x => x.SaleId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            builder
             .HasOne(p => p.Stock) 
            .WithMany()
            .HasForeignKey(p => p.StockId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);

            builder
                .HasOne(x => x.ReturnReason)
                .WithMany()
                .HasForeignKey(x => x.ReturnReasonId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            builder
             .HasOne(p => p.ReturnDestination)
            .WithMany()
            .HasForeignKey(p => p.ReturnDestinationId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);

            builder
             .HasOne(p => p.ReturnStatus)
            .WithMany()
            .HasForeignKey(p => p.ReturnStatusId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);

        }
    }
}
