using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Pharma263.Persistence.EntityConfigurations.Identity
{
    public class UserRoleConfiguration : IEntityTypeConfiguration<IdentityUserRole<string>>
    {
        public void Configure(EntityTypeBuilder<IdentityUserRole<string>> builder)
        {
            builder.ToTable("AspNetUserRoles", "Pharma263");

            builder.HasData(
                new IdentityUserRole<string>
                {
                    RoleId = "cbc43a8e-f7bb-4445-baaf-1add431ffbbf", //Admin
                    UserId = "8e445865-a24d-4543-a6c6-9443d048cdb9"
                },
                new IdentityUserRole<string>
                {
                    RoleId = "cbc43a8e-f7bb-4445-baaf-1add431ffbbf", //Admin
                    UserId = "8e445865-a24d-4543-a6c6-9443d0482205"
                },
                new IdentityUserRole<string>
                {
                    RoleId = "cac43a6e-f7bb-4448-baaf-1add431ccbbf", // Sales
                    UserId = "7e334968-23e4-4652-b7b7-8574d048cdc6"
                },
                new IdentityUserRole<string>
                {
                    RoleId = "cac43a6e-f7bb-4448-baaf-1add431aabbf", // Supervisor
                    UserId = "9e224968-33e4-4652-b7b7-8574d048cdb9"
                }
            );
        }
    }
}
