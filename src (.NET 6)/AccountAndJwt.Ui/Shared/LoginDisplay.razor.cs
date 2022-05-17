using AccountAndJwt.Ui.Services.Interfaces;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace AccountAndJwt.Ui.Shared
{
    public partial class LoginDisplay
    {
        // PROPERTIES /////////////////////////////////////////////////////////////////////////////
        [Inject]
        public IAuthorizationService AuthorizationService { get; set; }


        // FUNCTIONS //////////////////////////////////////////////////////////////////////////////
        private async Task LogOut(MouseEventArgs args)
        {
            await AuthorizationService.Logout();
        }
    }
}