using AccountAndJwt.Ui.Models;

namespace AccountAndJwt.Ui.Services.Interfaces
{
    public interface IAuthorizationService
    {
        User User { get; }

        Task Login(String login, String password, Boolean stayLoggedIn);
        Task Logout();
    }
}