using AccountAndJwt.AuthorizationService.Database;
using AccountAndJwt.AuthorizationService.Middleware;
using AccountAndJwt.AuthorizationService.Middleware.Cors;
using AccountAndJwt.Common.DependencyInjection;
using AccountAndJwt.Common.DependencyInjection.Modules;
using Autofac;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Serialization;
using System;
using System.Reflection;

namespace AccountAndJwt.IntegrationTests
{
    public class TestStartup
    {
        private readonly IConfiguration _configuration;


        public TestStartup(IConfiguration configuration)
        {
            _configuration = configuration;
        }


        // FUNCTIONS //////////////////////////////////////////////////////////////////////////////
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddResponseHandling();
            services.AddCors(CorsMiddleware.AddPolitics);
            services.AddEndpointsApiExplorer();
            services.AddConfiguredSwaggerGen();
            services.AddJwtAuthService(_configuration);
            services.AddHealthChecks();
            services.AddAuthentication();
            services.AddAuthorization();
            services.AddLogging();
            services
                .AddSingleton<IActionContextAccessor, ActionContextAccessor>()
                .AddScoped(x => x.GetRequiredService<IUrlHelperFactory>().GetUrlHelper(x.GetRequiredService<IActionContextAccessor>().ActionContext));
            services
                .AddControllers()
                .AddApplicationPart(Assembly.Load(new AssemblyName("AccountAndJwt.AuthorizationService")))
                .AddNewtonsoftJson(options =>
                {
                    options.SerializerSettings.ContractResolver = new DefaultContractResolver();
                });
        }
        public void ConfigureContainer(ContainerBuilder builder)
        {
            var authorizationServiceAssembly = Collector.GetAssembly("AccountAndJwt.AuthorizationService");
            builder.RegisterServiceConfiguration(_configuration, authorizationServiceAssembly);
            builder.RegisterServices(authorizationServiceAssembly);
            builder.RegisterModule(new InMemoryDatabaseModule(_configuration, Collector.GetAssembly("AccountAndJwt.AuthorizationService.Database")));
            builder.RegisterModule(new AutoMapperModule(Collector.LoadAssemblies("AccountAndJwt")));
        }
        public void Configure(IApplicationBuilder app, IServiceProvider services)
        {
            PopulateDatabase(services);

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseDeveloperExceptionPage();
            app.UseGlobalExceptionHandler();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHealthChecks("/healthz", new HealthCheckOptions());
                endpoints.MapControllers();
            });
        }
        public void PopulateDatabase(IServiceProvider services)
        {
            var configuration = services.GetRequiredService<IConfiguration>();
            var passwordSalt = configuration["AudienceConfig:PasswordSalt"];
            var context = services.GetRequiredService<DataContext>();

            context.Database.EnsureCreated();
            context.AddRoles();
            context.AddUsers(passwordSalt);
        }
    }
}