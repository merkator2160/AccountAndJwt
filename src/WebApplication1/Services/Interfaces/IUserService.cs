using AccountAndJwt.AuthorizationService.Services.Models;
using System;
using System.Threading.Tasks;

namespace AccountAndJwt.AuthorizationService.Services.Interfaces
{
	public interface IUserService
	{
		Task RegisterAsync(UserDto newUser);
		Task DeleteUserAsync(Int32 id);
		Task ChangeEmailAsync(Int32 userId, String newEmail);
		Task ChangeNameAsync(Int32 userId, String firstName, String lastName);
		Task RemoveRoleAsync(Int32 userId, String role);
		Task AddRoleAsync(Int32 userId, String role);
		Task<UserDto> GetUserAsync(Int32 id);
		Task<UserDto[]> GetAllUsersAsync();
		Task UpdatePasswordAsync(Int32 userId, String oldPassword, String newPassword);
	}
}