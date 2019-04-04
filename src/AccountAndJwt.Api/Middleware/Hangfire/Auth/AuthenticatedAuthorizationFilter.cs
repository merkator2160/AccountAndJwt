using Hangfire.Dashboard;
using System;

namespace AccountAndJwt.Api.Middleware.Hangfire.Auth
{
	internal class AuthenticatedAuthorizationFilter : IDashboardAuthorizationFilter
	{
		public Boolean Authorize(DashboardContext context)
		{
			var httpContext = context.GetHttpContext();

			// Allow all authenticated users to see the Dashboard (potentially dangerous).
			return httpContext.User.Identity.IsAuthenticated;
		}
	}
}