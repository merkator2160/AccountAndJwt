using AccountAndJwt.Contracts.Models.Api.Request;
using AccountAndJwt.Contracts.Models.Exceptions;
using AccountAndJwt.Ui.Clients.Interfaces;
using AccountAndJwt.Ui.Services.Interfaces;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace AccountAndJwt.Ui.Pages
{
    [Route("register")]
    public partial class Register
    {
        private readonly RegisterUserRequestAm _registerUserRequest = new();
        private Boolean _loading;
        private Boolean _stayLoggedIn;
        private String _errorMessage;


        // PROPERTIES /////////////////////////////////////////////////////////////////////////////
        [Inject]
        public NavigationManager Navigation { get; set; }

        [Inject]
        public IAuthorizationService AuthorizationService { get; set; }

        [Inject]
        public IAuthorizationHttpClient AuthorizationHttpClient { get; set; }

        [CascadingParameter]
        public Task<AuthenticationState> AuthState { get; set; }


        // FUNCTIONS //////////////////////////////////////////////////////////////////////////////
        protected override void OnInitialized()
        {
            var authState = AuthState.Result;
            var user = authState.User;

            if (user.Identity!.IsAuthenticated)
                Navigation.NavigateTo("/");
        }
        private async Task RegisterUser()
        {
            try
            {
                _loading = true;

                using (var response = await AuthorizationHttpClient.RegisterAsync(_registerUserRequest))
                {
                    if (!response.IsSuccessStatusCode)
                    {
                        // TODO: Add error popup window
                        throw new HttpServerException(HttpMethod.Post, response.StatusCode, response.Headers.Location!.AbsolutePath, await response.Content.ReadAsStringAsync());
                    }
                }

                await AuthorizationService.Login(_registerUserRequest.Login, _registerUserRequest.Password, _stayLoggedIn);
                Navigation.NavigateTo("/");
            }
            catch (Exception ex)
            {
                _errorMessage = ex.Message;
                _loading = false;

                StateHasChanged();
            }
        }
    }
}