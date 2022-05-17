using AccountAndJwt.Ui.Models;
using AccountAndJwt.Ui.Services.Interfaces;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace AccountAndJwt.Ui.Pages
{
    [Route("register")]
    public partial class Register
    {
        private readonly RegisterUserVm _registerUserRequest = new();
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
        private async Task RegisterUser()
        {
            try
            {
                _loading = true;
                await AuthenticationService.Login(_registerUserRequest.Login, _registerUserRequest.Password, _stayLoggedIn);

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