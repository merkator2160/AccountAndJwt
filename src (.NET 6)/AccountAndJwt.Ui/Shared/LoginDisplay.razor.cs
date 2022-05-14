using AccountAndJwt.Ui.Utilities;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;

namespace AccountAndJwt.Ui.Shared
{
    public partial class LoginDisplay
    {
        // PROPERTIES /////////////////////////////////////////////////////////////////////////////
        [Inject]
        public AuthenticationStateProvider AuthenticationService { get; set; }


        // FUNCTIONS //////////////////////////////////////////////////////////////////////////////
        private async Task LogOut(MouseEventArgs args)
        {
            await ((CustomAuthenticationStateProvider)AuthenticationService).Logout();
        }
    }
}