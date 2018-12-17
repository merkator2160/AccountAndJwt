using AccountAndJwt.Api.Services;
using AccountAndJwt.Api.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace AccountAndJwt.Api.Middleware
{
	public static class CustomServiceMiddleware
	{
		public static void AddCustomServices(this IServiceCollection services)
		{
			services
				.AddTransient<ITokenService, TokenService>()
				.AddTransient<IUserService, UserService>()
				.AddTransient<IEmailService, BasicEmailService>()
				.AddTransient<IValueService, ValueService>();
		}
	}
}