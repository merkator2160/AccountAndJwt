using System;
using System.Threading.Tasks;

namespace AccountAndJwt.Api.Database.Interfaces
{
	public interface IUnitOfWork
	{
		IValueRepository Values { get; }
		IUserRepository Users { get; }
		IRoleRepository Roles { get; }

		Int32 Commit();
		Task<Int32> CommitAsync();
	}
}