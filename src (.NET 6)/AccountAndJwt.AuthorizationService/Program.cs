using AccountAndJwt.AuthorizationService.Middleware;
using AccountAndJwt.AuthorizationService.Middleware.Cors;
using AccountAndJwt.AuthorizationService.Middleware.Hangfire;
using AccountAndJwt.Common.DependencyInjection;
using AccountAndJwt.Database.DependencyInjection;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Newtonsoft.Json.Serialization;
using System.Reflection;

namespace AccountAndJwt.AuthorizationService
{
    internal class Program
    {
        private static void Main(String[] args)
        {
            //TODO: We need logging here, with container support
            try
            {
                Run(args);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"{ex.Message}\r\n{ex.StackTrace}");
                throw;
            }
            finally
            {
                //TODO: Finalize global application logging for container
            }
        }
        private static void Run(String[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            ConfigureLogging(builder.Logging, builder.Configuration);
            ConfigureServices(builder.Services, builder.Configuration);
            builder.Host.UseServiceProviderFactory(ConfigureContainer(builder.Configuration));

            using (var app = builder.Build())
            {
                ConfigureWebApplication(app, builder.Configuration);
                app.Run();
            }
        }
        private static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddResponseHandling();
            services.AddCors(CorsMiddleware.AddPolitics);
            services.AddEndpointsApiExplorer();
            services.AddConfiguredSwaggerGen();
            services.AddHangfire(configuration);
            services.AddJwtAuthService(configuration);
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
        private static AutofacServiceProviderFactory ConfigureContainer(IConfiguration configuration)
        {
            return new AutofacServiceProviderFactory(containerBuilder =>
            {
                containerBuilder.RegisterLocalServices();
                containerBuilder.RegisterLocalHangfireJobs();
                containerBuilder.RegisterLocalConfiguration(configuration);

                containerBuilder.RegisterModule(new DatabaseModule(configuration));
                containerBuilder.RegisterModule(new AutoMapperModule(Collector.LoadAssemblies("AccountAndJwt")));
            });
        }
        private static void ConfigureLogging(ILoggingBuilder logging, IConfiguration configuration)
        {
            logging.ClearProviders();
            logging.SetMinimumLevel(LogLevel.Trace);
            logging.AddConfiguration(configuration.GetSection("Logging"));
            logging.AddDebug();
        }
        private static void ConfigureWebApplication(WebApplication app, IConfiguration configuration)
        {
            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseCors(CorsPolicies.Development);
                DatabaseModule.CheckDatabase(configuration, DatabaseModule.InitializeStrategy);
            }
            if (app.Environment.IsStaging())
            {
                app.UseDeveloperExceptionPage();
                app.UseCors(CorsPolicies.Staging);
                DatabaseModule.CheckDatabase(configuration, DatabaseModule.InitializeStrategy);
            }
            if (app.Environment.IsProduction())
            {
                app.UseHsts();
                app.UseCors(CorsPolicies.Production);
                DatabaseModule.CheckDatabase(configuration, DatabaseModule.InitializeStrategy);
            }

            var scope = app.Services.GetAutofacRoot();
            scope.RegisterJobActivator();

            app.UseHangfire();
            app.ConfigureHangfireJobs();
            app.UseConfiguredSwagger();

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHealthChecks("/healthz", new HealthCheckOptions());
                endpoints.MapControllers();
            });
            app.UseResponseCompression();
            app.UseGlobalExceptionHandler();
        }
    }
}