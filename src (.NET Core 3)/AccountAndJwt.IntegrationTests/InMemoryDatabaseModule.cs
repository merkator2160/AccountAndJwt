using AccountAndJwt.Common.DependencyInjection;
using AccountAndJwt.Database;
using Autofac;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Reflection;
using Module = Autofac.Module;

namespace AccountAndJwt.IntegrationTests
{
	internal class InMemoryDatabaseModule : Module
	{
		private const String _databaseName = "InMemoryDbForTesting";
		private readonly IConfiguration _configuration;
		private readonly Assembly _assembly;


		public InMemoryDatabaseModule(IConfiguration configuration, Assembly assembly)
		{
			_assembly = assembly;
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
			var contextOptionsBuilder = new DbContextOptionsBuilder()
				.UseInMemoryDatabase(_databaseName)
				.Options;

			builder.RegisterInstance(contextOptionsBuilder);
			builder
				.RegisterType<DataContext>()
				.AsSelf()
				.AsImplementedInterfaces()
				.InstancePerLifetimeScope();
		}
		public void RegisterRepositories(ContainerBuilder builder)
		{
			builder
				.RegisterAssemblyTypes(_assembly)
				.Where(t => t.Name.EndsWith("Repository"))
				.AsSelf()
				.AsImplementedInterfaces();

			builder
				.RegisterAssemblyTypes(_assembly)
				.Where(t => t.Name.EndsWith("Wrapper"))
				.AsImplementedInterfaces();

			builder
				.RegisterType<UnitOfWork>()
				.AsSelf()
				.AsImplementedInterfaces();
		}
	}
}