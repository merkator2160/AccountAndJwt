using AccountAndJwt.Ui.Clients.Interfaces;
using AccountAndJwt.Ui.Models;
using AccountAndJwt.Ui.Services.Interfaces;
using AccountAndJwt.Ui.Utilities;
using Blazored.SessionStorage;
using Microsoft.AspNetCore.Components.Authorization;

namespace AccountAndJwt.Ui.Services
{
    internal class UserService : IUserService
    {
        private readonly IAuthorizationHttpClient _authorizationHttpClient;
        private readonly ISessionStorageService _sessionStorageService;
        private readonly CustomAuthenticationStateProvider _authenticationStateProvider;


        public UserService(IAuthorizationHttpClient authorizationHttpClient, ISessionStorageService sessionStorageService, AuthenticationStateProvider authenticationStateProvider)
        {
            _authorizationHttpClient = authorizationHttpClient;
            _sessionStorageService = sessionStorageService;
            _authenticationStateProvider = (CustomAuthenticationStateProvider)authenticationStateProvider;
        }


        // IUserService ///////////////////////////////////////////////////////////////////////////
        public async Task<User[]> GetAll()
        {
            // TODO: Redesign + pagination
            return await _authorizationHttpClient.Get<User[]>("api/Account/GetAllUsers", _authenticationStateProvider.User.Tokens.AccessToken);
        }
    }
}