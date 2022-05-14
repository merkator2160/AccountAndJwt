using AccountAndJwt.Ui.Models;
using AccountAndJwt.Ui.Utilities;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace AccountAndJwt.Ui.Pages
{
    [Route("register")]
    public partial class Register
    {
        private readonly RegisterUserVm _registerUserRequest = new();
        private CustomAuthenticationStateProvider _authenticationStateProvider;
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


        // FUNCTIONS //////////////////////////////////////////////////////////////////////////////
        private async Task<bool> RegisterUser()
        {
            throw new NotImplementedException();

            //var returnedUser = await userService.RegisterUserAsync(_registerUserRequest);

            //if (returnedUser.EmailAddress != null)
            //{
            //    _authenticationStateProvider.Login();
            //    Navigation.NavigateTo("/");
            //}
            //else
            //{
            //    _errorMessage = "Invalid username or password";
            //}

            //return await Task.FromResult(true);
        }
    }
}