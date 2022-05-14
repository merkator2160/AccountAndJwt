using AccountAndJwt.Contracts.Models.Api;
using AccountAndJwt.Ui.Utilities;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace AccountAndJwt.Ui.Pages
{
    [Route("login")]
    public partial class Login
    {
        private readonly AuthorizeRequestAm _authorizeRequest = new();
        private CustomAuthenticationStateProvider _authenticationStateProvider;
        private Boolean _loading;
        private String _errorMessage;


        // PROPERTIES /////////////////////////////////////////////////////////////////////////////
        [Inject]
        public NavigationManager Navigation { get; set; }

        [Inject]
        public AuthenticationStateProvider AuthenticationService
        {
            get => _authenticationStateProvider;
            set => _authenticationStateProvider = (CustomAuthenticationStateProvider)value;
        }

        [CascadingParameter]
        public Task<AuthenticationState> AuthState { get; set; }


        // FUNCTIONS //////////////////////////////////////////////////////////////////////////////
        protected override void OnInitialized()
        {
            var authState = AuthState.Result;
            var user = authState.User;

            if (user.Identity.IsAuthenticated)
                Navigation.NavigateTo("/");
        }
        private async void HandleValidSubmit()
        {
            _loading = true;
            try
            {
                await _authenticationStateProvider.Login(_authorizeRequest.Login, _authorizeRequest.Password);

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