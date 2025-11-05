using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pharma263.Domain.Entities;
using Pharma263.Persistence.EntityConfigurations.Shared;

namespace Pharma263.Persistence.EntityConfigurations
{
    public class SaleEntityConfiguration : ConcurrencyTokenEntityConfiguration<Sales>, IEntityTypeConfiguration<Sales>
    {
        public void Configure(EntityTypeBuilder<Sales> builder)
        {
            builder.ToTable("Sales", "Pharma263");
            builder.HasKey(s => s.Id);

            builder.Property(s => s.SalesDate).HasColumnName("SalesDate").IsRequired();
            builder.Property(s => s.Notes).HasColumnName("Notes").HasMaxLength(200).IsRequired(false);
            builder.Property(s => s.Total).HasColumnName("Total").HasColumnType("DECIMAL(14,2)").IsRequired();            
            builder.Property(s => s.Discount).HasColumnName("Discount").HasColumnType("DECIMAL(14,2)");
            builder.Property(s => s.GrandTotal).HasColumnName("GrandTotal").HasColumnType("DECIMAL(14,2)");
            builder.Property(s => s.PaymentMethodId).HasColumnName("PaymentMethodId");
            builder.Property(s => s.SaleStatusId).HasColumnName("SaleStatusId");
            builder.Property(s => s.CustomerId).HasColumnName("CustomerId");

            ConfigureConcurrencyToken(builder);

            builder.HasIndex(b => new { b.Id, b.SalesDate }, "IX_Sale_Id_SaleDate").IsUnique();

            builder.HasOne(s => s.Customer)
               .WithMany()
               .HasForeignKey(s => s.CustomerId)
               .HasConstraintName("FK_Sales_Customers");

            builder.HasOne(s => s.SaleStatus)
               .WithMany()
               .HasForeignKey(s => s.SaleStatusId)
               .HasConstraintName("FK_Sales_SaleStatuses");

            builder.HasOne(s => s.PaymentMethod)
               .WithMany()
               .HasForeignKey(s => s.PaymentMethodId)
               .HasConstraintName("FK_Sales_PaymentMethods");
        }
    }
}
