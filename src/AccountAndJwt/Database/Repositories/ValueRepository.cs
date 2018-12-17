using AccountAndJwt.Api.Database.Interfaces;
using AccountAndJwt.Api.Database.Models;

namespace AccountAndJwt.Api.Database.Repositories
{
	internal class ValueRepository : EfRepositoryBase<ValueDb, DataContext>, IValueRepository
	{
		public ValueRepository(DataContext context) : base(context)
		{

		}
	}
}