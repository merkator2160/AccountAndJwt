using AccountAndJwt.Database.Models.Storage;
using System;
using System.Threading.Tasks;

namespace AccountAndJwt.Database.Interfaces
{
	public interface IValueRepository : IRepository<ValueDb>
	{
		Task<ValueDb> GetByValueAsync(Int32 value);
	}
}