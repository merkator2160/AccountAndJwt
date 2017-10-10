using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AccountAndJwt.Database.Mappings
{
    internal abstract class DbEntityMapBase<TEntity> where TEntity : class
    {
        public abstract void Configure(EntityTypeBuilder<TEntity> entityBuilder);
    }
}