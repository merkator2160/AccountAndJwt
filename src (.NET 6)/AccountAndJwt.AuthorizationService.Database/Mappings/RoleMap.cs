using AccountAndJwt.AuthorizationService.Database.Interfaces;
using AccountAndJwt.AuthorizationService.Database.Models.Storage;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AccountAndJwt.AuthorizationService.Database.Mappings
{
    internal class RoleMap : IEntityMap<RoleDb>
    {
        public void Configure(EntityTypeBuilder<RoleDb> entityBuilder)
        {
            entityBuilder
                .ToTable("Roles")
                .HasKey(p => p.Id);
        }
    }
}