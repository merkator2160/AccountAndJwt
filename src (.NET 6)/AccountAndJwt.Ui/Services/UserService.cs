using AccountAndJwt.Ui.Clients.Interfaces;
using AccountAndJwt.Ui.Models;
using AccountAndJwt.Ui.Services.Interfaces;

namespace AccountAndJwt.Ui.Services
{
    internal class UserService : IUserService
    {
        private readonly IAuthorizationHttpClient _authorizationHttpClient;
        private readonly IAuthorizationService _authorizationService;


        public UserService(IAuthorizationHttpClient authorizationHttpClient, IAuthorizationService authorizationService)
        {
            _authorizationHttpClient = authorizationHttpClient;
            _authorizationService = authorizationService;
        }


        // IUserService ///////////////////////////////////////////////////////////////////////////
        public async Task<User[]> GetAll()
        {
            // TODO: Redesign + pagination
            return await _authorizationHttpClient.Get<User[]>("api/Account/GetAllUsers", _authorizationService.User.ServerTokens.AccessToken);
        }
    }
}