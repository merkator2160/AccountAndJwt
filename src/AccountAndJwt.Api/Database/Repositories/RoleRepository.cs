using AccountAndJwt.Api.Database.Interfaces;
using AccountAndJwt.Api.Database.Models.Storage;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace AccountAndJwt.Api.Database.Repositories
{
	internal class RoleRepository : EfRepositoryBase<RoleDb, DataContext>, IRoleRepository
	{
		public RoleRepository(DataContext context) : base(context)
		{

		}


		// IRoleRepository ////////////////////////////////////////////////////////////////////////
		public Task<RoleDb> GetByNameEagerAsync(String roleName)
		{
			return Context.Roles
				.Include(p => p.UserRoles)
				.ThenInclude(p => p.User)
				.FirstOrDefaultAsync(p => p.Name == roleName);
		}
		public Task<RoleDb> GetByNameAsync(String roleName)
		{
			return Context.Roles.FirstOrDefaultAsync(p => p.Name == roleName);
		}
	}
}