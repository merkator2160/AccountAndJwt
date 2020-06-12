using AccountAndJwt.Database.Interfaces;
using AccountAndJwt.Database.Models.Storage;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace AccountAndJwt.Database.Repositories
{
	internal class ValueRepository : EfRepositoryBase<ValueDb, DataContext>, IValueRepository
	{
		public ValueRepository(DataContext context) : base(context)
		{

		}


		// IValueRepository ///////////////////////////////////////////////////////////////////////
		public Task<ValueDb> GetByValueAsync(Int32 value)
		{
			return Context.Values.FirstOrDefaultAsync(p => p.Value == value);
		}
	}
}