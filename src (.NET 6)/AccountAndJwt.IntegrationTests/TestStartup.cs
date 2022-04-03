using AccountAndJwt.AuthorizationService.Middleware;
using AccountAndJwt.AuthorizationService.Middleware.Cors;
using AccountAndJwt.Common.Config;
using AccountAndJwt.Common.DependencyInjection;
using AccountAndJwt.Database;
using Autofac;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
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
        private readonly IWebHostEnvironment _env;


        public TestStartup(IWebHostEnvironment env)
        {
            _env = env;
            _configuration = CustomConfigurationProvider.CreateConfiguration(env.EnvironmentName, env.WebRootPath);
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
            builder.RegisterModule(new InMemoryDatabaseModule(_configuration, Collector.GetAssembly("AccountAndJwt.Database")));
            builder.RegisterModule(new AutoMapperModule(Collector.LoadAssemblies("AccountAndJwt")));
        }
        public void Configure(IApplicationBuilder app, IServiceProvider serviceProvider)
        {
            using (var context = serviceProvider.GetRequiredService<DataContext>())
            {
                context.Database.EnsureCreated();
                var salt = _configuration["AudienceConfig:PasswordSalt"];
                context.AddRoles();
                context.AddUsers(salt);
            }

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
    }
}