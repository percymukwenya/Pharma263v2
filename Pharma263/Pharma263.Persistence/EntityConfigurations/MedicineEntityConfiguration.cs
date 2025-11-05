using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pharma263.Domain.Entities;
using Pharma263.Persistence.EntityConfigurations.Shared;

namespace Pharma263.Persistence.EntityConfigurations
{
    public class MedicineEntityConfiguration : ConcurrencyTokenEntityConfiguration<Medicine>, IEntityTypeConfiguration<Medicine>
    {
        public void Configure(EntityTypeBuilder<Medicine> builder)
        {
            builder.ToTable("Medicine", "Pharma263");
            builder.HasKey(b => b.Id);

            builder.Property(b => b.Name).HasColumnName("Name").HasMaxLength(150).IsRequired();
            builder.Property(b => b.GenericName).HasColumnName("GenericName").HasMaxLength(50).IsRequired(false);
            builder.Property(b => b.Brand).HasColumnName("Brand").HasMaxLength(50).IsRequired(false);
            builder.Property(b => b.Manufacturer).HasColumnName("Manufacturer").HasMaxLength(50).IsRequired(false);
            builder.Property(b => b.DosageForm).HasColumnName("DosageForm").HasMaxLength(50).IsRequired(false);
            builder.Property(b => b.PackSize).HasColumnName("PackSize").HasMaxLength(50).IsRequired(false);
            builder.Property(b => b.QuantityPerUnit).HasColumnName("QuantityPerUnit").IsRequired();

            ConfigureConcurrencyToken(builder);

            builder.HasIndex(b => b.Name, "UX_Medicine_Name").IsUnique();
        }
    }
}
