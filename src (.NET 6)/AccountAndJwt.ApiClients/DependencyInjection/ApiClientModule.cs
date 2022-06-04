using AccountAndJwt.ApiClients.Redis;
using AccountAndJwt.Common.DependencyInjection;
using Autofac;
using Microsoft.Extensions.Configuration;
using System.Reflection;
using Module = Autofac.Module;

namespace AccountAndJwt.ApiClients.DependencyInjection
{
    public class ApiClientModule : Module
    {
        private readonly IConfiguration _configuration;
        private readonly Assembly _currentAssembly;


        public ApiClientModule(IConfiguration configuration)
        {
            _currentAssembly = Assembly.GetAssembly(typeof(ApiClientModule));
            _configuration = configuration;
        }


        // FUNCTIONS //////////////////////////////////////////////////////////////////////////////
        protected override void Load(ContainerBuilder builder)
        {
            RegisterClients(builder);
            builder.RegisterServiceConfiguration(_configuration, _currentAssembly);
        }
        public void RegisterClients(ContainerBuilder builder)
        {
            builder.RegisterType<RedisClient>()
                .AsSelf()
                .AsImplementedInterfaces()
                .SingleInstance();

            builder
                .RegisterAssemblyTypes(_currentAssembly)
                .Where(p => p.Name.EndsWith("Client"))
                .Except<RedisClient>()
                .AsSelf()
                .AsImplementedInterfaces();
        }
    }
}