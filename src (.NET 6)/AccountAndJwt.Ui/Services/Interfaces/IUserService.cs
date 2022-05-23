using AccountAndJwt.Contracts.Models.Api;

namespace AccountAndJwt.Ui.Services.Interfaces
{
    internal interface IUserService
    {
        Task<UserAm[]> GetAll();
    }
}