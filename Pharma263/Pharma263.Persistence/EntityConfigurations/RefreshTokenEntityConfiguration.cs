using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pharma263.Domain.Entities;

namespace Pharma263.Persistence.EntityConfigurations
{
    public class RefreshTokenEntityConfiguration : IEntityTypeConfiguration<RefreshToken>
    {
        public void Configure(EntityTypeBuilder<RefreshToken> builder)
        {
            builder.ToTable("RefreshTokens", "Pharma263");
            builder.HasKey(b => b.Id);

            builder.Property(b => b.Token).HasColumnName("Token").HasMaxLength(500).IsRequired();
            builder.Property(b => b.UserId).HasColumnName("UserId").HasMaxLength(450).IsRequired();
            builder.Property(b => b.ExpiryDate).HasColumnName("ExpiryDate").IsRequired();
            builder.Property(b => b.IsRevoked).HasColumnName("IsRevoked").IsRequired();
            builder.Property(b => b.RevokedDate).HasColumnName("RevokedDate").IsRequired(false);
            builder.Property(b => b.RevokedByIp).HasColumnName("RevokedByIp").HasMaxLength(50).IsRequired(false);
            builder.Property(b => b.CreatedByIp).HasColumnName("CreatedByIp").HasMaxLength(50).IsRequired(false);
            builder.Property(b => b.ReplacedByToken).HasColumnName("ReplacedByToken").HasMaxLength(500).IsRequired(false);

            // Indexes for performance
            builder.HasIndex(b => b.Token, "IX_RefreshTokens_Token");
            builder.HasIndex(b => b.UserId, "IX_RefreshTokens_UserId");
            builder.HasIndex(b => b.ExpiryDate, "IX_RefreshTokens_ExpiryDate");

            // Relationship with ApplicationUser
            builder
                .HasOne(r => r.User)
                .WithMany()
                .HasForeignKey(r => r.UserId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            // Ignore computed properties
            builder.Ignore(r => r.IsExpired);
            builder.Ignore(r => r.IsActive);
        }
    }
}
