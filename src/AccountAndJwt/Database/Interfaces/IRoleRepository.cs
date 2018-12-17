using AccountAndJwt.Api.Database.Models;
using System;

namespace AccountAndJwt.Api.Database.Interfaces
{
	public interface IRoleRepository : IRepository<RoleDb>
	{
		RoleDb GetByNameEager(String roleName);
		RoleDb GetByName(String roleName);
	}
}