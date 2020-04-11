using AccountAndJwt.Database.Interfaces;
using AccountAndJwt.Database.Models.Storage;

namespace AccountAndJwt.Database.Repositories
{
	internal class ValueRepository : EfRepositoryBase<ValueDb, DataContext>, IValueRepository
	{
		public ValueRepository(DataContext context) : base(context)
		{

		}


		// IValueRepository ///////////////////////////////////////////////////////////////////////
	}
}