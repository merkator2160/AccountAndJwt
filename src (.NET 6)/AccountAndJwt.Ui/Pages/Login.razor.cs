using AccountAndJwt.Contracts.Models.Api;
using AccountAndJwt.Ui.Services.Interfaces;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace AccountAndJwt.Ui.Pages
{
    [Route("login")]
    public partial class Login
    {
        private readonly AuthorizeRequestAm _authorizeRequest = new();
        private Boolean _loading;
        private Boolean _stayLoggedIn;
        private String _errorMessage;


        // PROPERTIES /////////////////////////////////////////////////////////////////////////////
        [Inject]
        public NavigationManager Navigation { get; set; }

        [Inject]
        public IAuthorizationService AuthenticationService { get; set; }

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
        private async void HandleValidSubmit()
        {
            _loading = true;
            try
            {
                await AuthenticationService.Login(_authorizeRequest.Login, _authorizeRequest.Password, _stayLoggedIn);

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