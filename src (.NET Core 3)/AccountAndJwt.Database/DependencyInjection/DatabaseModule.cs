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
		public const String _defaultConnectionStringName = "DefaultConnection";
		private readonly IConfiguration _configuration;
		private readonly Assembly _currentAssembly;


		public DatabaseModule(IConfiguration configuration)
		{
			_currentAssembly = Assembly.GetExecutingAssembly();
			_configuration = configuration;

			ConnectionString = configuration.GetConnectionString(_defaultConnectionStringName);
			Config = configuration.GetSection("DatabaseConfig").Get<DatabaseConfig>();
		}
		public DatabaseModule(String connectionString)
		{
			_currentAssembly = Assembly.GetExecutingAssembly();
			ConnectionString = connectionString;
		}


		// PROPERTIES /////////////////////////////////////////////////////////////////////////////
		public Boolean IsMigration { get; set; }
		public String ConnectionString { get; }
		public DatabaseConfig Config { get; }


		// COMPONENT REGISTRATION /////////////////////////////////////////////////////////////////
		protected override void Load(ContainerBuilder builder)
		{
			RegisterContext(builder);
			RegisterRepositories(builder);
			builder.RegisterLocalConfiguration(_configuration);
		}
		public void RegisterContext(ContainerBuilder builder)
		{
			var contextOptionsBuilder = new DbContextOptionsBuilder()
				.UseSqlServer(ConnectionString, opt => opt
					.EnableRetryOnFailure()
					.CommandTimeout(IsMigration ? Config.MigrationTimeout : Config.CommandTimeout))
				.UseSnakeCaseNamingConvention()
#if DEBUG
				.AddInterceptors(new HintCommandInterceptor())
#endif
				.Options;

			builder.RegisterInstance(contextOptionsBuilder);
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
		public static void CreateDatabase(IConfiguration configurationService, Action<DataContext, String> strategy)
		{
			var connectionString = configurationService.GetConnectionString(_defaultConnectionStringName);
			var optionsBuilder = new DbContextOptionsBuilder().UseSqlServer(connectionString);
			using(var context = new DataContext(optionsBuilder.Options))
			{
				strategy.Invoke(context, configurationService["AudienceConfig:PasswordSalt"]);
			}
		}


		// STRATEGY ///////////////////////////////////////////////////////////////////////////////
		public static void InitializeStrategy(DataContext context, String salt)
		{
			context.Database.EnsureCreated();
			context.AddInitialData(salt);
		}
		public static void DropCreateInitializeStrategy(DataContext context, String salt)
		{
			context.Database.EnsureDeleted();
			context.Database.EnsureCreated();
			context.AddInitialData(salt);
		}
		public static void MigrateInitializeStrategy(DataContext context, String salt)
		{
			var pendingMigrations = context.Database.GetPendingMigrations().ToArray();
			if(pendingMigrations.Length > 0)
				context.Database.Migrate();

			context.AddInitialData(salt);
		}
	}
}