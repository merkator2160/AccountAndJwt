using AccountAndJwt.Contracts.Const;
using AccountAndJwt.Ui.Clients.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;

namespace AccountAndJwt.Ui.Pages
{
    [Route("userEditor")]
    [Authorize(Roles = Role.Admin)]
    public partial class UserEditor
    {

        // PROPERTIES /////////////////////////////////////////////////////////////////////////////
        [Inject]
        public IAuthorizationHttpClient AuthorizationHttpClient { get; set; }


        // FUNCTIONS //////////////////////////////////////////////////////////////////////////////
        protected override async Task OnInitializedAsync()
        {

        }
    }
}