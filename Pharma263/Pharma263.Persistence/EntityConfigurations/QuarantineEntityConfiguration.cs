using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pharma263.Domain.Entities;
using Pharma263.Persistence.EntityConfigurations.Shared;

namespace Pharma263.Persistence.EntityConfigurations
{
    public class QuarantineEntityConfiguration : ConcurrencyTokenEntityConfiguration<Quarantine>, IEntityTypeConfiguration<Quarantine>
    {
        public void Configure(EntityTypeBuilder<Quarantine> builder)
        {

            builder.ToTable("Quarantine", "Pharma263");
            builder.HasKey(b => b.Id);
            builder.Property(b => b.TotalQuantity).HasColumnName("TotalQuantity").IsRequired();
            builder.Property(b => b.MedicineId).HasColumnName("MedicineId").IsRequired();

            ConfigureConcurrencyToken(builder);

            builder
                .HasOne(x => x.Medicine)
                .WithMany()
                .HasForeignKey(x => x.MedicineId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
