using AccountAndJwt.Common.DependencyInjection;
using AccountAndJwt.Database.Interceptors;
using AccountAndJwt.Database.Models.Config;
using Autofac;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;
using System.Reflection;
using Module = Autofac.Module;

namespace AccountAndJwt.Database.DependencyInjection
{
	public class DatabaseModule : Module
	{
		public const String ConnectionStringName = "DefaultConnection";
		private readonly IConfiguration _configuration;
		private readonly Assembly _currentAssembly;


		public DatabaseModule(IConfiguration configuration)
		{
			_currentAssembly = Assembly.GetExecutingAssembly();
			_configuration = configuration;
		}


		// COMPONENT REGISTRATION /////////////////////////////////////////////////////////////////
		protected override void Load(ContainerBuilder builder)
		{
			RegisterContext(builder);
			RegisterRepositories(builder);
			builder.RegisterLocalConfiguration(_configuration);
		}
		public void RegisterContext(ContainerBuilder builder)
		{
			builder.RegisterInstance(CreateContextOptions(_configuration));
			builder.RegisterType<DataContext>()
				.AsSelf()
				.AsImplementedInterfaces()
				.InstancePerLifetimeScope();
		}
		public void RegisterRepositories(ContainerBuilder builder)
		{
			builder
				.RegisterAssemblyTypes(_currentAssembly)
				.Where(t => t.Name.EndsWith("Repository"))
				.AsSelf()
				.AsImplementedInterfaces();

			builder
				.RegisterAssemblyTypes(_currentAssembly)
				.Where(t => t.Name.EndsWith("Wrapper"))
				.AsImplementedInterfaces();

			builder
				.RegisterType<UnitOfWork>()
				.AsSelf()
				.AsImplementedInterfaces();
		}


		// CONTEXT FUNCTIONS //////////////////////////////////////////////////////////////////////
		public static void CheckDatabase(IConfiguration configurationService, Action<DataContext, String> strategy)
		{
			using(var context = CreateMigrationContext(configurationService))
			{
				strategy.Invoke(context, configurationService["AudienceConfig:PasswordSalt"]);
			}
		}
		public static DataContext CreateMigrationContext(IConfiguration configurationService)
		{
			return new DataContext(CreateContextOptions(configurationService, true));
		}
		private static DbContextOptions CreateContextOptions(IConfiguration configurationService, Boolean isMigrationMode = false)
		{
			var connectionString = configurationService.GetConnectionString(ConnectionStringName);
			var config = configurationService.GetSection("DatabaseConfig").Get<DatabaseConfig>();

			return new DbContextOptionsBuilder()
				.UseSqlServer(connectionString, opt => opt
					.EnableRetryOnFailure()
					.CommandTimeout(isMigrationMode ? config.MigrationTimeout : config.CommandTimeout))
				//.UseSnakeCaseNamingConvention()		// Usual case for PostgreSQL, I left it here for further usage, because this project is my common micro-service prototype
#if DEVELOPMENT
				.AddInterceptors(new HintCommandInterceptor())
				.EnableSensitiveDataLogging()
				.EnableDetailedErrors()
#endif
				.Options;
		}


		// STRATEGY ///////////////////////////////////////////////////////////////////////////////
		public static void InitializeStrategy(DataContext context, String salt)
		{
			context.Database.EnsureCreated();
			context.AddAllInitialData(salt);
		}
		public static void DropCreateInitializeStrategy(DataContext context, String salt)
		{
			context.Database.EnsureDeleted();
			context.Database.EnsureCreated();
			context.AddAllInitialData(salt);
		}
		public static void MigrateInitializeStrategy(DataContext context, String salt)
		{
			var pendingMigrations = context.Database.GetPendingMigrations().ToArray();
			if(pendingMigrations.Length > 0)
				context.Database.Migrate();

			context.AddAllInitialData(salt);
		}
	}
}