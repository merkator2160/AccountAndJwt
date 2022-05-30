using AccountAndJwt.AuthorizationService.Database.Interfaces;
using AccountAndJwt.AuthorizationService.Database.Models.Storage;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AccountAndJwt.AuthorizationService.Database.Mappings
{
    internal class ValueMap : IEntityMap<ValueDb>
    {
        public void Configure(EntityTypeBuilder<ValueDb> entityBuilder)
        {
            entityBuilder
                .ToTable("Values")
                .HasKey(p => p.Id);

            entityBuilder
                .Property(p => p.Commentary)
                .HasMaxLength(50);
        }
    }
}