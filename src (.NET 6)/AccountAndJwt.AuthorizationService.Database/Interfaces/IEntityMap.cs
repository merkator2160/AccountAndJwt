using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AccountAndJwt.AuthorizationService.Database.Interfaces
{
    public interface IEntityMap<TEntity> where TEntity : class
    {
        void Configure(EntityTypeBuilder<TEntity> entityBuilder);
    }
}