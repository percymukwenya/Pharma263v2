using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Pharma263.Persistence.EntityConfigurations.Identity
{
    public class RoleConfiguration : IEntityTypeConfiguration<IdentityRole>
    {
        public void Configure(EntityTypeBuilder<IdentityRole> builder)
        {
            builder.ToTable("AspNetRoles", "Pharma263");
            builder.HasKey(p => p.Id);

            builder.HasData(
                new IdentityRole
                {
                    Id = "cac43a6e-f7bb-4448-baaf-1add431ccbbf",
                    Name = "Sales",
                    NormalizedName = "SALES"
                },
                new IdentityRole
                {
                    Id = "cac43a6e-f7bb-4448-baaf-1add431aabbf",
                    Name = "Supervisor",
                    NormalizedName = "SUPERVISOR"
                },
                new IdentityRole
                {
                    Id = "cbc43a8e-f7bb-4445-baaf-1add431ffbbf",
                    Name = "Administrator",
                    NormalizedName = "ADMINISTRATOR"
                }
             );
        }
    }
}
