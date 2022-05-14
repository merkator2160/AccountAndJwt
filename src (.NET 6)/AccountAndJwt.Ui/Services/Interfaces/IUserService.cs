using AccountAndJwt.Ui.Models;

namespace AccountAndJwt.Ui.Services.Interfaces
{
    internal interface IUserService
    {
        Task<User[]> GetAll();
    }
}