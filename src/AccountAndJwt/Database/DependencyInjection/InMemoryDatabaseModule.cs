using AccountAndJwt.Api.Core.DependencyInjection;
using AccountAndJwt.Api.Database.Interfaces;
using Autofac;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Reflection;
using Module = Autofac.Module;

namespace AccountAndJwt.Api.Database.DependencyInjection
{
	internal class InMemoryDatabaseModule : Module
	{
		private const String _databaseName = "InMemoryDbForTesting";
		private readonly IConfiguration _configuration;
		private readonly Assembly _assembly;


		public InMemoryDatabaseModule(IConfiguration configuration)
		{
			_assembly = Assembly.GetExecutingAssembly();
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
			builder.RegisterType<DataContext>().InstancePerLifetimeScope();
		}
		public void RegisterRepositories(ContainerBuilder builder)
		{
			builder
				.RegisterAssemblyTypes(_assembly)
				.Where(t => t.Name.EndsWith("Repository"))
				.AsClosedTypesOf(typeof(IRepository<>));

			builder
				.RegisterType<UnitOfWork>()
				.AsSelf()
				.AsImplementedInterfaces();
		}
	}
}