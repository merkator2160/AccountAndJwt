using AccountAndJwt.Database.Interfaces;
using AccountAndJwt.Database.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AccountAndJwt.Database.Mappings
{
    internal class UserRoleMap : IEntityMap<UserRoleDb>
    {
        public void Configure(EntityTypeBuilder<UserRoleDb> entityBuilder)
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