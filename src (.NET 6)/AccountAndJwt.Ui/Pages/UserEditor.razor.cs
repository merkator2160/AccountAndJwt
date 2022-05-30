using AccountAndJwt.ApiClients.Http.Authorization.Interfaces;
using AccountAndJwt.Contracts.Const;
using Blazorise;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;

namespace AccountAndJwt.Ui.Pages
{
    [Route("userEditor")]
    [Authorize(Roles = Role.Admin)]
    public partial class UserEditor
    {
        private Modal _modalRef;


        // PROPERTIES /////////////////////////////////////////////////////////////////////////////
        [Inject]
        public IAuthorizationHttpClient AuthorizationHttpClient { get; set; }


        // FUNCTIONS //////////////////////////////////////////////////////////////////////////////
        protected override async Task OnInitializedAsync()
        {

        }
        private Task ShowModal()
        {
            return _modalRef.Show();
        }
        private Task OnCloseClicked()
        {
            return _modalRef.Hide();
        }
        private Task OnSaveChangesClicked()
        {
            return _modalRef.Hide();
        }
    }
}