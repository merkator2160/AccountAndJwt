using AccountAndJwt.Contracts.Models.Api;
using AccountAndJwt.Ui.Clients.Interfaces;
using AccountAndJwt.Ui.Services.Interfaces;

namespace AccountAndJwt.Ui.Services
{
    public class UserService : IUserService
    {
        private readonly IAuthorizationHttpClient _authorizationHttpClient;
        private readonly IAuthorizationService _authorizationService;


        public UserService(IAuthorizationHttpClient authorizationHttpClient, IAuthorizationService authorizationService)
        {
            _authorizationHttpClient = authorizationHttpClient;
            _authorizationService = authorizationService;
        }


        // IUserService ///////////////////////////////////////////////////////////////////////////
        public async Task<UserAm[]> GetAll()
        {
            // TODO: Redesign + pagination
            return await _authorizationHttpClient.Get<UserAm[]>("api/Account/GetAllUsers", _authorizationService.User.ServerTokens.AccessToken);
        }
    }
}