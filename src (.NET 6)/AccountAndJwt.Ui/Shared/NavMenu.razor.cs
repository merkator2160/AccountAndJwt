namespace AccountAndJwt.Ui.Shared
{
    public partial class NavMenu
    {
        private Boolean _collapseNavMenu = true;



        // PROPERTIES /////////////////////////////////////////////////////////////////////////////
        private String NavMenuCssClass => _collapseNavMenu ? "collapse" : null;



        // FUNCTIONS //////////////////////////////////////////////////////////////////////////////
        private void ToggleNavMenu()
        {
            _collapseNavMenu = !_collapseNavMenu;
        }
    }
}