using AccountAndJwt.Contracts.Models.Api;
using AccountAndJwt.Ui.Clients.Interfaces;
using AccountAndJwt.Ui.Models;
using Blazored.SessionStorage;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http.Json;
using System.Security.Claims;

namespace AccountAndJwt.Ui.Utilities
{
    internal class CustomAuthenticationStateProvider : AuthenticationStateProvider
    {
        private const String _sessionStorageUserKey = "user";

        private readonly ISessionStorageService _sessionStorageService;
        private readonly IAuthorizationHttpClient _authorizationHttpClient;
        private readonly JwtTokenParser _jwtTokenParser;
        private readonly NavigationManager _navigationManager;

        private User _user;


        public CustomAuthenticationStateProvider(ISessionStorageService sessionStorageService, IAuthorizationHttpClient authorizationHttpClient, JwtTokenParser jwtTokenParser, NavigationManager navigationManager)
        {
            _user = CreateUnauthorizedUser();
            _sessionStorageService = sessionStorageService;
            _authorizationHttpClient = authorizationHttpClient;
            _jwtTokenParser = jwtTokenParser;
            _navigationManager = navigationManager;
        }


        // PROPERTIES /////////////////////////////////////////////////////////////////////////////
        public User User => _user;


        // FUNCTIONS //////////////////////////////////////////////////////////////////////////////
        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var userFromStorage = await _sessionStorageService.GetItemAsync<User>(_sessionStorageUserKey);
            if (userFromStorage == null)
            {
                return await Task.FromResult(new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity())));
            }

            var now = DateTime.UtcNow;
            if (now > userFromStorage.TokenExpirationTimeUtc)
            {
                return await Task.FromResult(new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity())));
            }

            var identity = CreateClaimsIdentity(userFromStorage);
            var claimsPrincipal = new ClaimsPrincipal(identity);

            return await Task.FromResult(new AuthenticationState(claimsPrincipal));
        }
        public async Task Login(String login, String password)
        {
            var response = await _authorizationHttpClient.AuthorizeByCredentialsAsync(new AuthorizeRequestAm()
            {
                Login = login,
                Password = password
            });
            if (!response.IsSuccessStatusCode)
                throw new ApplicationException(await response.Content.ReadAsStringAsync());

            var authorizeResponse = await response.Content.ReadFromJsonAsync<AuthorizeResponseAm>();

            _user = CreateAuthorizedUser(authorizeResponse);
            await _sessionStorageService.SetItemAsync(_sessionStorageUserKey, _user);

            var identity = CreateClaimsIdentity(_user);
            var claimsPrincipal = new ClaimsPrincipal(identity);

            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(claimsPrincipal)));
            _navigationManager.NavigateTo("/");
        }
        public async Task Logout()
        {
            await _sessionStorageService.RemoveItemAsync(_sessionStorageUserKey);

            var identity = new ClaimsIdentity();
            var user = new ClaimsPrincipal(identity);

            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(user)));
            _navigationManager.NavigateTo("/");
        }


        // FUNCTIONS //////////////////////////////////////////////////////////////////////////////
        private User CreateAuthorizedUser(AuthorizeResponseAm authorizeResponse)
        {
            var token = _jwtTokenParser.ParseToken(authorizeResponse.AccessToken);

            Console.WriteLine(token.Header.TokenAlgorithm);
            Console.WriteLine(token.Payload.TokenExpirationTime);
            Console.WriteLine(token.Payload.GetClaimValueByKey("exp"));
            Console.WriteLine(token.Payload.ClaimExists("key").ToString());
            Console.WriteLine(token.ToJson());

            return new User()
            {
                Id = Int32.Parse(token.Payload.Claims[ClaimTypes.Sid]),
                FirstName = token.Payload.Claims[ClaimTypes.Name],
                LastName = token.Payload.Claims[ClaimTypes.Surname],
                Email = token.Payload.Claims[ClaimTypes.Email],
                TokenExpirationTimeUtc = token.Payload.TokenExpirationTime,
                IsAuthorized = true,
                Tokens = authorizeResponse
            };
        }
        private User CreateUnauthorizedUser()
        {
            return new User()
            {
                IsAuthorized = true,
            };
        }
        private ClaimsIdentity CreateClaimsIdentity(User user)
        {
            var token = _jwtTokenParser.ParseToken(user.Tokens.AccessToken);
            var claimList = token.Payload.Claims.Select(x => new Claim(x.Key, x.Value)).ToArray();

            return new ClaimsIdentity(claimList, "apiauth_type"); ;
        }
    }
}
