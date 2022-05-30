using AccountAndJwt.AuthorizationService.Database.Interfaces;
using AccountAndJwt.AuthorizationService.Database.Models.Storage;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AccountAndJwt.AuthorizationService.Database.Mappings
{
    internal class UserMap : IEntityMap<UserDb>
    {
        public void Configure(EntityTypeBuilder<UserDb> entityBuilder)
        {
            entityBuilder
                .ToTable("Users")
                .HasKey(p => p.Id);
        }
    }
}