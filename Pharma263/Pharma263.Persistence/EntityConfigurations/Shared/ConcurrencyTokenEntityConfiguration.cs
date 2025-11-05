using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pharma263.Domain.Common;

namespace Pharma263.Persistence.EntityConfigurations.Shared
{
    public class ConcurrencyTokenEntityConfiguration<T> where T : ConcurrencyTokenEntity
    {
        public void ConfigureConcurrencyToken(EntityTypeBuilder<T> builder)
        {
            builder.Property(b => b.CreatedBy).HasColumnName("CreatedBy").HasMaxLength(255).HasDefaultValue("'System'").IsRequired();
            builder.Property(b => b.CreatedDate).HasColumnName("CreatedDate").HasDefaultValueSql("SYSDATETIMEOFFSET()").IsRequired();
            builder.Property(b => b.ModifiedBy).HasColumnName("ModifiedBy").HasMaxLength(255).IsRequired(false);
            builder.Property(b => b.ModifiedDate).HasColumnName("ModifiedDate").IsRequired(false);
            builder.Property(b => b.IsDeleted).HasColumnName("IsDeleted").HasDefaultValue(false).IsRequired();

            builder.Property(b => b.TimeStamp).HasColumnName("TimeStamp").IsRequired().IsRowVersion();

            builder.HasQueryFilter(p => !p.IsDeleted);
        }
    }
}
