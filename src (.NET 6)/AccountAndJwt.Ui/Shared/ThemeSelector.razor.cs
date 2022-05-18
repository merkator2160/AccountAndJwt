using AccountAndJwt.Ui.Services.Interfaces;
using Microsoft.AspNetCore.Components;

namespace AccountAndJwt.Ui.Shared
{
    public partial class ThemeSelector
    {
        // PROPERTIES /////////////////////////////////////////////////////////////////////////////
        [Inject]
        public IRadzenThemeService RadzenThemeService { get; set; }


        // FUNCTIONS //////////////////////////////////////////////////////////////////////////////
        void ChangeTheme(Object value)
        {
            RadzenThemeService.Change((String)value);
        }
    }
}