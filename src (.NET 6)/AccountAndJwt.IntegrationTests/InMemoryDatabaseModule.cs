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
        private readonly String _databaseName;
        private readonly IConfiguration _configuration;
        private readonly Assembly _assembly;
        private readonly Boolean _uniqueDbEachTime;


        /// <summary>
        /// Different DB names means different instances in memory, it's an equivalent of connection string. With the same name all tests will share the same DB, which may lead to test errors.
        /// </summary>
		public InMemoryDatabaseModule(IConfiguration configuration, Assembly assembly, Boolean uniqueDbEachTime = true, String databaseName = "InMemoryDbForTesting")
        {
            _databaseName = databaseName;
            _assembly = assembly;
            _uniqueDbEachTime = uniqueDbEachTime;
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
            if (_uniqueDbEachTime)
                builder.Register(sp => new DbContextOptionsBuilder().UseInMemoryDatabase($"{_databaseName}-{Guid.NewGuid()}").Options).AsSelf();
            else
                builder.RegisterInstance(new DbContextOptionsBuilder().UseInMemoryDatabase(_databaseName).Options);

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