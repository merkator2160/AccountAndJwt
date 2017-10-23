using AccountAndJwt.Database.Interfaces;
using AccountAndJwt.Models.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AccountAndJwt.Database.Mappings
{
    internal class ValueMap : IEntityMap<ValueDb>
    {
        public void Configure(EntityTypeBuilder<ValueDb> entityBuilder)
        {
            entityBuilder.ToTable("Values");
            entityBuilder.HasKey(p => p.Id);
        }
    }
}