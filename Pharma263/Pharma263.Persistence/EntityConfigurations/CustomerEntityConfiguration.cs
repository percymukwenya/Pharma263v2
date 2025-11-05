using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pharma263.Domain.Entities;
using Pharma263.Persistence.EntityConfigurations.Shared;

namespace Pharma263.Persistence.EntityConfigurations
{
    public class CustomerEntityConfiguration : ConcurrencyTokenEntityConfiguration<Customer>, IEntityTypeConfiguration<Customer>
    {
        public void Configure(EntityTypeBuilder<Customer> builder)
        {
            builder.ToTable("Customer", "Pharma263");
            builder.HasKey(b => b.Id);

            builder.Property(b => b.Name).HasColumnName("Name").HasMaxLength(150).IsRequired();
            builder.Property(b => b.Email).HasColumnName("Email").HasMaxLength(150).IsRequired();
            builder.Property(b => b.Phone).HasColumnName("Phone").HasMaxLength(20).IsRequired();
            builder.Property(b => b.PhysicalAddress).HasColumnName("PhysicalAddress").HasMaxLength(200).IsRequired();
            builder.Property(b => b.DeliveryAddress).HasColumnName("DeliveryAddress").HasMaxLength(200).IsRequired(false);
            builder.Property(b => b.MCAZLicence).HasColumnName("MCAZLicence").HasMaxLength(50).IsRequired(false);
            builder.Property(b => b.HPALicense).HasColumnName("HPALicense").HasMaxLength(50).IsRequired(false);
            builder.Property(b => b.VATNumber).HasColumnName("VATNumber").HasMaxLength(50).IsRequired(false);
            builder.Property(b => b.CustomerTypeId).HasColumnName("CustomerTypeId").IsRequired();

            ConfigureConcurrencyToken(builder);

            //Indexes
            builder.HasIndex(b => b.Name, "UX_Customer_Name").IsUnique();

            //One To Many
            builder
                .HasOne(c => c.CustomerType)
                .WithMany()
                .HasForeignKey(c => c.CustomerTypeId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
