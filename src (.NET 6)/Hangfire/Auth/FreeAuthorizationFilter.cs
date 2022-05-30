using Hangfire.Dashboard;

namespace Hangfire.Auth
{
    public class FreeAuthorizationFilter : IDashboardAuthorizationFilter
    {
        public Boolean Authorize(DashboardContext context)
        {
            return true;    // Allow all users to see the Dashboard (extremely dangerous)
        }
    }
}