using AccountAndJwt.Database.Interfaces;
using AccountAndJwt.Models.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AccountAndJwt.Database.Mappings
{
    internal class UserMap : IEntityMap<UserDb>
    {
        public void Configure(EntityTypeBuilder<UserDb> entityBuilder)
        {
            entityBuilder.ToTable("Users");
            entityBuilder.HasKey(p => p.Id);
        }
    }
}