using AccountAndJwt.Api.Database.Interfaces;
using AccountAndJwt.Api.Database.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace AccountAndJwt.Api.Database.Repositories
{
	internal class RoleRepository : EfRepositoryBase<RoleDb, DataContext>, IRoleRepository
	{
		public RoleRepository(DataContext context) : base(context)
		{

		}


		// IRoleRepository ////////////////////////////////////////////////////////////////////////
		public RoleDb GetByNameEager(String roleName)
		{
			return Context.Roles
				.Include(p => p.UserRoles)
				.ThenInclude(p => p.User)
				.FirstOrDefault(p => p.RoleName == roleName);
		}
		public RoleDb GetByName(String roleName)
		{
			return Context.Roles.FirstOrDefault(p => p.RoleName == roleName);
		}
	}
}