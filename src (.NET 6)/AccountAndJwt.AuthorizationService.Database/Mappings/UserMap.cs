using AccountAndJwt.AuthorizationService.Database.Interfaces;
using AccountAndJwt.AuthorizationService.Database.Models.Storage;
using AccountAndJwt.Contracts.Const;
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

            entityBuilder
                .Property(p => p.Login)
                .HasMaxLength(Limits.User.LoginMaxLength);

            entityBuilder
                .Property(p => p.FirstName)
                .HasMaxLength(Limits.User.FirstNameMaxLength);

            entityBuilder
                .Property(p => p.LastName)
                .HasMaxLength(Limits.User.LastNameMaxLength);
        }
    }
}