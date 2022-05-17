using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace AccountAndJwt.Ui.Pages
{
    [Route("counter")]
    public partial class Counter
    {
        private Int32 _currentCount;


        // PROPERTIES /////////////////////////////////////////////////////////////////////////////
        [CascadingParameter]
        public Task<AuthenticationState> AuthState { get; set; }


        // FUNCTIONS //////////////////////////////////////////////////////////////////////////////
        private async Task IncrementCount()
        {
            var authState = await AuthState;
            var user = authState.User;

            if (user.Identity.IsAuthenticated)
                _currentCount++;
            else
                _currentCount--;
        }
    }
}