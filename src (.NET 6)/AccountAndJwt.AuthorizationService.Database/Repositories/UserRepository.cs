using AccountAndJwt.AuthorizationService.Database.Interfaces;
using AccountAndJwt.AuthorizationService.Database.Models;
using AccountAndJwt.AuthorizationService.Database.Models.Storage;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using X.PagedList;

namespace AccountAndJwt.AuthorizationService.Database.Repositories
{
    internal class UserRepository : EfRepositoryBase<UserDb, DataContext>, IUserRepository
    {
        private readonly IMapper _mapper;


        public UserRepository(DataContext context, IMapper mapper) : base(context)
        {
            _mapper = mapper;
        }


        // IUserRepository ////////////////////////////////////////////////////////////////////////
        public Task<Boolean> UserExistsAsync(String login)
        {
            return Context.Users.AnyAsync(p => p.Login.Equals(login));
        }
        public Task<Boolean> UserRoleExistsAsync(Int32 userId, Int32 roleId)
        {
            return Context.UserRoles.AnyAsync(p => p.RoleId == roleId && p.UserId == userId);
        }

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
        public async Task<PagedUserDb> GetPagedEagerAsync(Int32 pageSize, Int32 pageNumber)
        {
            var query = Context.Users
               .Include(p => p.UserRoles)
               .ThenInclude(p => p.Role);

            return _mapper.Map<PagedUserDb>(await query.ToPagedListAsync(pageNumber, pageSize));
        }
        public Task<RoleDb[]> GetAvailableRolesAsync()
        {
            return Context.Roles.ToArrayAsync();
        }
        public Task AddUserRoleAsync(Int32 userId, Int32 roleId)
        {
            return Context.UserRoles.AddAsync(new UserRoleDb()
            {
                UserId = userId,
                RoleId = roleId
            }).AsTask();
        }
        public void DeleteUserRole(UserRoleDb userRole)
        {
            Context.UserRoles.Remove(userRole);
        }
        public async Task DeleteUserRoleAsync(Int32 userId, Int32 roleId)
        {
            var requestedUserRole = await Context.UserRoles.FirstAsync(p => p.RoleId == roleId && p.UserId == userId);
            Context.UserRoles.Remove(requestedUserRole);
        }
        public Task<RoleDb> GetRoleWithUserAsync(Int32 roleId)
        {
            return Context.Roles
                .Include(p => p.UserRoles)
                .ThenInclude(p => p.User)
                .FirstOrDefaultAsync(p => p.Id == roleId);
        }
    }
}