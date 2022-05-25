using AccountAndJwt.Contracts.Models.Api.Response;
using AccountAndJwt.Ui.Clients.Interfaces;
using AccountAndJwt.Ui.Models;
using AccountAndJwt.Ui.Services.Interfaces;
using AccountAndJwt.Ui.Utilities.TokenParser;
using Blazored.SessionStorage;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace AccountAndJwt.Ui.Services
{
    /// <summary>
    /// This might be useful in future: https://medium.com/nuances-of-programming/4-%D1%81%D0%BF%D0%BE%D1%81%D0%BE%D0%B1%D0%B0-%D0%BE%D0%B1%D0%BC%D0%B5%D0%BD%D0%B0-%D0%B4%D0%B0%D0%BD%D0%BD%D1%8B%D0%BC%D0%B8-%D0%BC%D0%B5%D0%B6%D0%B4%D1%83-%D0%B2%D0%BA%D0%BB%D0%B0%D0%B4%D0%BA%D0%B0%D0%BC%D0%B8-%D0%B1%D1%80%D0%B0%D1%83%D0%B7%D0%B5%D1%80%D0%B0-%D0%B2-%D1%80%D0%B5%D0%B6%D0%B8%D0%BC%D0%B5-%D1%80%D0%B5%D0%B0%D0%BB%D1%8C%D0%BD%D0%BE%D0%B3%D0%BE-%D0%B2%D1%80%D0%B5%D0%BC%D0%B5%D0%BD%D0%B8-4d6e81b0f934
    /// </summary>
    internal class AuthorizationService : AuthenticationStateProvider, IAuthorizationService
    {
        private const String _userKey = "user";
        private const String _authType = "apiauth_type";

        private readonly ISessionStorageService _sessionStorageService;
        private readonly ILocalStorageService _localStorageService;
        private readonly IAuthorizationHttpClient _authorizationHttpClient;
        private readonly NavigationManager _navigationManager;

        private User _user;


        public AuthorizationService(
            ISessionStorageService sessionStorageService,
            ILocalStorageService localStorageService,
            IAuthorizationHttpClient authorizationHttpClient,
            NavigationManager navigationManager)
        {
            _user = CreateUnauthorizedUser();
            _sessionStorageService = sessionStorageService;
            _localStorageService = localStorageService;
            _authorizationHttpClient = authorizationHttpClient;
            _navigationManager = navigationManager;
        }


        // PROPERTIES /////////////////////////////////////////////////////////////////////////////
        public User User => _user;


        // OVERRIDE ///////////////////////////////////////////////////////////////////////////////
        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var stayLoggedIn = true;
            var userFromStorage = await _localStorageService.GetItemAsync<User>(_userKey);
            if (userFromStorage == null)
            {
                stayLoggedIn = false;
                userFromStorage = await _sessionStorageService.GetItemAsync<User>(_userKey);
                if (userFromStorage == null)
                    return await Task.FromResult(new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity())));
            }

            var now = DateTime.UtcNow;
            if (now > userFromStorage.AccessTokenExpirationTimeUtc)
            {
                userFromStorage.ServerTokens.AccessToken = await _authorizationHttpClient.RefreshTokenAsync(_user.ServerTokens.RefreshToken);
                UpdateUserInStorageAsync(stayLoggedIn);
            }

            _user = userFromStorage;

            return await Task.FromResult(new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity(_user.ParsedToken.Payload.ClaimDictionary.ToClaims(), _authType))));
        }


        // IAuthorizationService //////////////////////////////////////////////////////////////////
        public async Task Login(String login, String password, Boolean stayLoggedIn)
        {
            var authorizeResponse = await _authorizationHttpClient.AuthorizeByCredentialsAsync(login, password);

            _user = CreateAuthorizedUser(authorizeResponse);
            UpdateUserInStorageAsync(stayLoggedIn);

            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity(_user.ParsedToken.Payload.ClaimDictionary.ToClaims(), _authType)))));
            _navigationManager.NavigateTo("/");
        }
        public async Task Logout()
        {
            _authorizationHttpClient.RevokeTokenAsync(_user.ServerTokens.RefreshToken);
            await _localStorageService.RemoveItemAsync(_userKey);
            await _sessionStorageService.RemoveItemAsync(_userKey);

            _user = CreateUnauthorizedUser();
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()))));

            _navigationManager.NavigateTo("/");
        }


        // FUNCTIONS //////////////////////////////////////////////////////////////////////////////
        private User CreateAuthorizedUser(AuthorizeResponseAm authorizeResponse)
        {
            var token = authorizeResponse.AccessToken.ParseToken();

            return new User()
            {
                Id = token.Payload.Sid,
                FirstName = token.Payload.FirstName,
                LastName = token.Payload.LastName,
                Email = token.Payload.Email,
                AccessTokenExpirationTimeUtc = token.Payload.TokenExpirationTime,
                IsAuthorized = true,
                ServerTokens = authorizeResponse,
                ParsedToken = token
            };
        }
        private User CreateUnauthorizedUser()
        {
            return new User()
            {
                IsAuthorized = true,
            };
        }
        private async void UpdateUserInStorageAsync(Boolean stayLoggedIn)
        {
            if (stayLoggedIn)
                await _localStorageService.SetItemAsync(_userKey, _user);
            else
                await _sessionStorageService.SetItemAsync(_userKey, _user);
        }
    }
}
