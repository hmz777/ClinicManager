using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ClinicProject.Server.Data.SeedData
{
    public class RoleSeed : IEntityTypeConfiguration<IdentityRole>
    {
        internal const string AdminRoleId = "3a073d1c-2a14-47d5-b2b1-94a6a5376418";

        public void Configure(EntityTypeBuilder<IdentityRole> builder)
        {
            builder.HasData(new IdentityRole
            {
                Id = AdminRoleId,
                Name = "Admin",
                NormalizedName = "ADMIN"
            });
        }
    }
}
