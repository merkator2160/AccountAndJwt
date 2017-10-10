using AccountAndJwt.Models.Database;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AccountAndJwt.Database.Mappings
{
    internal class UserRoleMap : DbEntityMapBase<UserRoleDb>
    {
        public override void Configure(EntityTypeBuilder<UserRoleDb> entityBuilder)
        {
            entityBuilder
                .HasKey(p => new { p.RoleId, p.UserId });

            entityBuilder
                .HasOne(p => p.Role)
                .WithMany(p => p.UserRoles)
                .HasForeignKey(p => p.RoleId);

            entityBuilder
                .HasOne(p => p.User)
                .WithMany(p => p.UserRoles)
                .HasForeignKey(p => p.UserId);
        }
    }
}