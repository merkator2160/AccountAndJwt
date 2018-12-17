using AccountAndJwt.Api.Database;
using AccountAndJwt.Api.Database.Interfaces;
using AccountAndJwt.Api.Database.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AccountAndJwt.Api.Middleware
{
	internal static class DatabaseMiddleware
	{
		public static void AddDatabase(this IServiceCollection services, IConfiguration configurationService)
		{
			services.AddEntityFrameworkInMemoryDatabase().AddDbContext<DataContext>(c => c.UseInMemoryDatabase(configurationService["DatabaseConfig:Name"]));

			services.AddTransient<IUserRepository, UserRepository>();
			services.AddTransient<IRoleRepository, RoleRepository>();
			services.AddTransient<IValueRepository, ValueRepository>();
			services.AddTransient<IUnitOfWork, UnitOfWork>();
		}
	}
}