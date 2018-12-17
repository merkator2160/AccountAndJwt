using AccountAndJwt.Middleware.Configs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AccountAndJwt.Middleware
{
    public static class ConfigMiddleware
    {
        public static void AddConfigurations(this IServiceCollection services, IConfiguration configuration)
        {
            services
                .Configure<AudienceConfig>(configuration.GetSection("AudienceConfig"))
                .Configure<EmailServiceConfig>(configuration.GetSection("EmailServiceConfig"));
        }
    }
}