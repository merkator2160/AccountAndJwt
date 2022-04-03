using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AccountAndJwt.Common.Interfaces
{
	public interface IEntityMap<TEntity> where TEntity : class
	{
		void Configure(EntityTypeBuilder<TEntity> entityBuilder);
	}
}