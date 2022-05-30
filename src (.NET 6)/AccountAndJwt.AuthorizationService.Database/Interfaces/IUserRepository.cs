using AccountAndJwt.AuthorizationService.Database.Models;
using AccountAndJwt.AuthorizationService.Database.Models.Storage;

namespace AccountAndJwt.AuthorizationService.Database.Interfaces
{
    public interface IUserRepository : IRepository<UserDb>
    {
        Task<Boolean> UserExistsAsync(String login);
        Task<Boolean> UserRoleExistsAsync(Int32 userId, Int32 roleId);

        Task<UserDb> GetByLoginEagerAsync(String login);
        Task<UserDb> GetByRefreshTokenEagerAsync(String refreshToken);
        Task<UserDb> GetEagerAsync(Int32 id);
        Task<PagedUserDb> GetPagedEagerAsync(Int32 pageSize, Int32 pageNumber);
        Task<RoleDb[]> GetAvailableRolesAsync();
        Task AddUserRoleAsync(Int32 userId, Int32 roleId);
        void DeleteUserRole(UserRoleDb userRole);
        Task DeleteUserRoleAsync(Int32 userId, Int32 roleId);
        Task<RoleDb> GetRoleWithUserAsync(Int32 roleId);
    }
}