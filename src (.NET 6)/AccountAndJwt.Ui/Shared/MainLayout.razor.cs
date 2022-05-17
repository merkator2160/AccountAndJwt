using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace AccountAndJwt.Ui.Shared
{
    /// <summary>
    /// https://question-it.com/questions/2098835/kak-poluchit-svezhuju-informatsiju-avtorizovan-li-polzovatel
    /// </summary>
    public partial class MainLayout
    {
        // PROPERTIES /////////////////////////////////////////////////////////////////////////////
        [Inject]
        public AuthenticationStateProvider AuthenticationStateProvider { get; set; }


        // FUNCTIONS //////////////////////////////////////////////////////////////////////////////
        protected override void OnInitialized()
        {
            AuthenticationStateProvider.AuthenticationStateChanged += OnAuthenticationStateChanged;
        }
        private void OnAuthenticationStateChanged(Task<AuthenticationState> authenticationStateTask)
        {
            StateHasChanged();
        }
    }
}