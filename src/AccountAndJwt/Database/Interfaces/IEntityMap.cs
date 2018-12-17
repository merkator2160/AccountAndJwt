using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AccountAndJwt.Api.Database.Interfaces
{
	internal interface IEntityMap<TEntity> where TEntity : class
	{
		void Configure(EntityTypeBuilder<TEntity> entityBuilder);
	}
}