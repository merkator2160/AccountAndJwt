using AccountAndJwt.Database.Interfaces;
using AccountAndJwt.Models.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AccountAndJwt.Database.Mappings
{
    internal class RoleMap : IEntityMap<RoleDb>
    {
        public void Configure(EntityTypeBuilder<RoleDb> entityBuilder)
        {
            entityBuilder.ToTable("UserRoles");
            entityBuilder.HasKey(p => p.Id);
        }
    }
}