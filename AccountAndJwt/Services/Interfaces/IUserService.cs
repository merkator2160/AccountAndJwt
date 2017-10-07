using AccountAndJwt.Models.Service;
using System;
using System.Threading.Tasks;

namespace AccountAndJwt.Services.Interfaces
{
    public interface IUserService
    {
        void Register(UserDto newUser);
        void DeleteUser(String id);
        Task ChangeEmailAsync(String userId, String newEmail);
        void ChangeName(String userId, String firstName, String lastName);
    }
}