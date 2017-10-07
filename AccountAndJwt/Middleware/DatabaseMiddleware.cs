using AccountAndJwt.Database;
using AccountAndJwt.Database.Interfaces;
using AccountAndJwt.Database.Repositorues;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AccountAndJwt.Middleware
{
    internal static class DatabaseMiddleware
    {
        public static void AddDatabase(this IServiceCollection services, IConfiguration configurationService)
        {
            services.AddEntityFrameworkInMemoryDatabase().AddDbContext<DataContext>(c => c.UseInMemoryDatabase(configurationService["DatabaseConfig:Name"]));

            services.AddTransient<ITokenRepository, TokenRepository>();
            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<IValueRepository, ValueRepository>();
            services.AddTransient<IUnitOfWork, UnitOfWork>();
        }
    }
}