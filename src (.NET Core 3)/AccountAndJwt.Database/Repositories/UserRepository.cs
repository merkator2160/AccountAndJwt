using AccountAndJwt.Database.Interfaces;
using AccountAndJwt.Database.Models.Storage;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace AccountAndJwt.Database.Repositories
{
	internal class UserRepository : EfRepositoryBase<UserDb, DataContext>, IUserRepository
	{
		public UserRepository(DataContext context) : base(context)
		{

		}


		// IUserRepository ////////////////////////////////////////////////////////////////////////
		public Task<UserDb> GetByLoginEagerAsync(String login)
		{
			return Context.Users
				.Include(p => p.UserRoles)
				.ThenInclude(p => p.Role)
				.FirstOrDefaultAsync(p => p.Login == login);
		}
		public Task<UserDb> GetByRefreshTokenEagerAsync(String refreshToken)
		{
			return Context.Users
				.Include(p => p.UserRoles)
				.ThenInclude(p => p.Role)
				.FirstOrDefaultAsync(p => p.RefreshToken == refreshToken);
		}
		public Task<UserDb> GetEagerAsync(Int32 id)
		{
			return Context.Users
				.Include(p => p.UserRoles)
				.ThenInclude(p => p.Role)
				.FirstOrDefaultAsync(p => p.Id == id);
		}
		public Task<UserDb[]> GetAllEagerAsync()
		{
			return Context.Users
				.Include(p => p.UserRoles)
				.ThenInclude(p => p.Role)
				.ToArrayAsync();
		}
		public Task AddRoleAsync(Int32 userId, Int32 roleId)
		{
			var userRole = new UserRoleDb()
			{
				UserId = userId,
				RoleId = roleId
			};
			return Context.UserRoles.AddAsync(userRole).AsTask();
		}
		public async Task DeleteRoleAsync(Int32 userId, Int32 roleId)
		{
			var requestedUserRole = await Context.UserRoles.FirstAsync(p => p.RoleId == roleId && p.UserId == userId);
			Context.UserRoles.Remove(requestedUserRole);
		}
		public Task<RoleDb> GetRoleWithUserAsync(String roleName)
		{
			return Context.Roles
				.Include(p => p.UserRoles)
				.ThenInclude(p => p.User)
				.FirstOrDefaultAsync(p => p.Name == roleName);
		}
	}
}