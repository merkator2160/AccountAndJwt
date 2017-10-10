using AccountAndJwt.Models.Service;
using System;
using System.Threading.Tasks;

namespace AccountAndJwt.Services.Interfaces
{
    public interface IUserService
    {
        void Register(UserDto newUser);
        void DeleteUser(Int32 id);
        Task ChangeEmailAsync(Int32 userId, String newEmail);
        void ChangeName(Int32 userId, String firstName, String lastName);
        void RemoveRole(Int32 userId, String role);
        void AddRole(Int32 userId, String role);
        UserDto GetUser(Int32 id);
        UserDto[] GetAllUsers();
        void UpdatePassword(Int32 userId, String oldPassword, String newPassword);
    }
}