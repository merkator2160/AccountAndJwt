using AccountAndJwt.ApiClients.Http.Authorization.Interfaces;
using AccountAndJwt.Common.Exceptions;
using AccountAndJwt.Contracts.Models.Api.Request;
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

        [Inject]
        public IBrowserPopupService BrowserPopupService { get; set; }


        // FUNCTIONS //////////////////////////////////////////////////////////////////////////////
        protected override void OnInitialized()
        {
            CheckUserAuthentication();
        }
        private void CheckUserAuthentication()
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

                await AuthorizationHttpClient.RegisterAsync(_registerUserRequest);
                await AuthorizationService.Login(_registerUserRequest.Login, _registerUserRequest.Password,
                    _stayLoggedIn);

                Navigation.NavigateTo("/");
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
            finally
            {
                _loading = false;
            }
        }
        private void HandleException(Exception ex)
        {
            if (ex is HttpServerException httpServerException)
            {
                if ((Int32)httpServerException.StatusCode == 460)
                {
                    _errorMessage = ex.Message;
                    StateHasChanged();
                }
                else
                {
                    BrowserPopupService.Alert(httpServerException.ToString());
                }

                return;
            }

            BrowserPopupService.Alert($"{ex.Message}\r\n{ex.StackTrace}");
        }
    }
}