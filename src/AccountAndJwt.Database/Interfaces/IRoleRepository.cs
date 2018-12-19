using AccountAndJwt.Database.Models.Storage;
using System;
using System.Threading.Tasks;

namespace AccountAndJwt.Database.Interfaces
{
	public interface IRoleRepository : IRepository<RoleDb>
	{
		Task<RoleDb> GetByNameEagerAsync(String roleName);
		Task<RoleDb> GetByNameAsync(String roleName);
	}
}