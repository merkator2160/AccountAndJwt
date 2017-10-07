using AccountAndJwt.Services;
using AccountAndJwt.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace AccountAndJwt.Middleware
{
    public static class CustomServiceMiddleware
    {
        public static void AddCustomServices(this IServiceCollection services)
        {
            services
                .AddTransient<ITokenService, TokenService>()
                .AddTransient<IUserService, UserService>()
                .AddTransient<IEmailService, BasicEmailService>();
        }
    }
}