using ClinicProject.Server.Data.DBModels.AppUsers;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ClinicProject.Server.Data.SeedData
{
    public class UserSeed : IEntityTypeConfiguration<ApplicationUser>
    {
        internal const string MasterAdminId = "7fcaf4d6-57c8-4b99-8843-20831be97ee0";

        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            var user = new ApplicationUser
            {
                Id = MasterAdminId,
                UserName = "masteradmin",
                NormalizedUserName = "MASTERADMIN",
                Email = "admin@admin.com",
                NormalizedEmail = "ADMIN@ADMIN.COM",
                EmailConfirmed = true,
                SecurityStamp = new Guid().ToString("D")
            };

            user.PasswordHash = HashPassword(user);

            builder.HasData(user);
        }

        string HashPassword(ApplicationUser user)
        {
            var hasher = new PasswordHasher<ApplicationUser>();

            return hasher.HashPassword(user, "password");
        }
    }
}
