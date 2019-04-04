using Hangfire.Dashboard;
using System;

namespace AccountAndJwt.Api.Middleware.Hangfire.Auth
{
	internal class FreeAuthorizationFilter : IDashboardAuthorizationFilter
	{
		public Boolean Authorize(DashboardContext context)
		{
			return true;    // Allow all users to see the Dashboard (extremely dangerous)
		}
	}
}