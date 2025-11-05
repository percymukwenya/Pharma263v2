using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pharma263.Domain.Entities;
using Pharma263.Persistence.EntityConfigurations.Shared;

namespace Pharma263.Persistence.EntityConfigurations
{
    public class SupplierEntityConfiguration : ConcurrencyTokenEntityConfiguration<Supplier>, IEntityTypeConfiguration<Supplier>
    {
        public void Configure(EntityTypeBuilder<Supplier> builder)
        {
            builder.ToTable("Supplier", "Pharma263");
            builder.HasKey(b => b.Id);
            builder.Property(b => b.Name).HasColumnName("Name").HasMaxLength(150).IsRequired();
            builder.Property(b => b.Email).HasColumnName("Email").HasMaxLength(150).IsRequired();
            builder.Property(b => b.Phone).HasColumnName("Phone").HasMaxLength(20).IsRequired();
            builder.Property(b => b.Address).HasColumnName("Address").HasMaxLength(200).IsRequired();
            builder.Property(b => b.Notes).HasColumnName("Notes").HasMaxLength(200).IsRequired(false);
            builder.Property(b => b.MCAZLicence).HasColumnName("MCAZLicence").HasMaxLength(50).IsRequired(false);
            builder.Property(b => b.BusinessPartnerNumber).HasColumnName("BusinessPartnerNumber").HasMaxLength(50).IsRequired(false);
            builder.Property(b => b.VATNumber).HasColumnName("VATNumber").HasMaxLength(50).IsRequired(false);

            ConfigureConcurrencyToken(builder);

            builder.HasIndex(b => b.Name, "UX_Supplier_Name").IsUnique();
        }
    }
}
