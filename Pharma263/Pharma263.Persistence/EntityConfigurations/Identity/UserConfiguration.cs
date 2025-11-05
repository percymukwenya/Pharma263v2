using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pharma263.Domain.Entities;

namespace Pharma263.Persistence.EntityConfigurations.Identity
{
    public class UserConfiguration : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            builder.ToTable("AspNetUsers", "Pharma263");
            builder.HasKey(p => p.Id);

            var hasher = new PasswordHasher<ApplicationUser>();
            builder.HasData(
                 new ApplicationUser
                 {
                     Id = "8e445865-a24d-4543-a6c6-9443d048cdb9",
                     Email = "admin@pharma263.com",
                     NormalizedEmail = "ADMIN@PHARMA263.COM",
                     FirstName = "Admin",
                     LastName = "Admin",
                     UserName = "admin",
                     NormalizedUserName = "ADMIN",
                     PasswordHash = hasher.HashPassword(null, "P@ssword1"),
                     EmailConfirmed = true
                 },
                 new ApplicationUser
                 {
                     Id = "8e445865-a24d-4543-a6c6-9443d0482205",
                     Email = "pfmukwenya@gmail.com",
                     NormalizedEmail = "PFMUKWENYA@GMAIL.COM",
                     FirstName = "Percy",
                     LastName = "Mukwenya",
                     UserName = "Percy",
                     NormalizedUserName = "PERCY",
                     PasswordHash = hasher.HashPassword(null, "P@ssword1"),
                     EmailConfirmed = true
                 },
                 new ApplicationUser
                 {
                     Id = "9e224968-33e4-4652-b7b7-8574d048cdb9",
                     Email = "supervisor@pharma263.com",
                     NormalizedEmail = "SUPERVISOR@PHARMA263.COM",
                     FirstName = "Supervisor",
                     LastName = "Supervisor",
                     UserName = "Supervisor",
                     NormalizedUserName = "SUPERVISOR",
                     PasswordHash = hasher.HashPassword(null, "P@ssword1"),
                     EmailConfirmed = true
                 },
                 new ApplicationUser
                 {
                     Id = "7e334968-23e4-4652-b7b7-8574d048cdc6",
                     Email = "sales@pharma263.com",
                     NormalizedEmail = "SALES@PHARMA263.COM",
                     FirstName = "Sales",
                     LastName = "Sales",
                     UserName = "Sales",
                     NormalizedUserName = "SALES",
                     PasswordHash = hasher.HashPassword(null, "P@ssword1"),
                     EmailConfirmed = true
                 }
            );
        }
    }
}
