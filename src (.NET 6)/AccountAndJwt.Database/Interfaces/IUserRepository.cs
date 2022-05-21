using AccountAndJwt.Database.Models.Storage;

namespace AccountAndJwt.Database.Interfaces
{
    public interface IUserRepository : IRepository<UserDb>
    {
        Task<UserDb> GetByLoginEagerAsync(String login);
        Task<UserDb> GetByRefreshTokenEagerAsync(String refreshToken);
        Task<UserDb> GetEagerAsync(Int32 id);
        Task<UserDb[]> GetAllEagerAsync();
        Task<RoleDb[]> GetAvailableRolesAsync();
        Task AddRoleAsync(Int32 userId, Int32 roleId);
        Task DeleteRoleAsync(Int32 userId, Int32 roleId);
        Task<RoleDb> GetRoleWithUserAsync(String roleName);
    }
}