using Microsoft.AspNetCore.Components;

namespace AccountAndJwt.Ui.Pages
{
    [Route("counter")]
    public partial class Counter
    {
        private Int32 _currentCount;


        // FUNCTIONS //////////////////////////////////////////////////////////////////////////////
        private void IncrementCount()
        {
            _currentCount++;
        }
    }
}