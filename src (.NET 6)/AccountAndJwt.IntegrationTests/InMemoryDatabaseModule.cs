using AccountAndJwt.AuthorizationService.Database;
using Autofac;
using CustomConfiguration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Reflection;
using Module = Autofac.Module;

namespace AccountAndJwt.IntegrationTests
{
    internal class InMemoryDatabaseModule : Module
    {
        private readonly String _databaseName;
        private readonly IConfiguration _configuration;
        private readonly Assembly _assembly;


        /// <summary>
        /// Different DB names means different instances in memory, it's an equivalent of connection string. With the same name all tests will share the same DB, which may lead to test errors.
        /// </summary>
		public InMemoryDatabaseModule(IConfiguration configuration, Assembly assembly, String databaseName = "InMemoryDbForTesting")
        {
            _databaseName = databaseName;
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
            var dbSuffix = Guid.NewGuid().ToString().Replace("-", "");

            builder.Register(sp => new DbContextOptionsBuilder().UseInMemoryDatabase($"{_databaseName}-{dbSuffix}").Options).AsSelf().SingleInstance();
            builder
                .RegisterType<DataContext>()
                .AsSelf()
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