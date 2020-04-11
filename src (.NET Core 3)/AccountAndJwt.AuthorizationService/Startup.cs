using AccountAndJwt.AuthorizationService.Middleware;
using AccountAndJwt.AuthorizationService.Middleware.Cors;
using AccountAndJwt.AuthorizationService.Middleware.Hangfire;
using AccountAndJwt.Common.Config;
using AccountAndJwt.Common.DependencyInjection;
using AccountAndJwt.Database.DependencyInjection;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace AccountAndJwt.AuthorizationService
{
	internal class Startup
	{
		private readonly IConfiguration _configuration;
		private readonly IWebHostEnvironment _env;


		public Startup(IWebHostEnvironment env)
		{
			_env = env;
			_configuration = CustomConfigurationProvider.CreateConfiguration(env.EnvironmentName, env.ContentRootPath);
		}


		// FUNCTIONS //////////////////////////////////////////////////////////////////////////////
		public void ConfigureServices(IServiceCollection services)
		{
			services
				.AddSingleton<IActionContextAccessor, ActionContextAccessor>()
				.AddScoped(x => x.GetRequiredService<IUrlHelperFactory>().GetUrlHelper(x.GetRequiredService<IActionContextAccessor>().ActionContext));
			services.AddCors(CorsMiddleware.AddPolitics);
			services.AddConfiguredSwaggerGen();
			services.AddHangfire(_configuration);
			services.AddJwtAuthService(_configuration);
			services.AddHealthChecks();
			services.ConfigureResponseHandling();
			services
				.AddControllers()
				//.AddNewtonsoftJson(options => options.SerializerSettings.ContractResolver = new DefaultContractResolver() { NamingStrategy = new SnakeCaseNamingStrategy() })
				.SetCompatibilityVersion(CompatibilityVersion.Version_3_0);
		}
		public void ConfigureContainer(ContainerBuilder builder)
		{
			builder.RegisterLocalServices();
			builder.RegisterLocalHangfireJobs();
			builder.RegisterLocalConfiguration(_configuration);

			builder.RegisterModule<NLogModule>();
			builder.RegisterModule(new DatabaseModule(_configuration));
			builder.RegisterModule(new AutoMapperModule(Collector.LoadAssemblies("AccountAndJwt")));
		}
		public void Configure(IApplicationBuilder app)
		{
			var scope = app.ApplicationServices.GetAutofacRoot();
			scope.RegisterJobActivator();

			if(_env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
				app.UseCors(CorsPolicies.Development);
				//DatabaseModule.CreateDatabase(_configuration, DatabaseModule.InitializeStrategy);
			}
			if(_env.IsStaging())
			{
				app.UseDeveloperExceptionPage();
				app.UseCors(CorsPolicies.Staging);
				//DatabaseModule.CreateDatabase(_configuration, DatabaseModule.InitializeStrategy);
			}
			if(_env.IsProduction())
			{
				app.UseCors(CorsPolicies.Production);
				//DatabaseModule.CreateDatabase(_configuration, DatabaseModule.InitializeStrategy);
			}

			app.UseHsts();
			app.UseHttpsRedirection();
			app.UseConfiguredSwagger();
			app.UseHangfire();
			app.ConfigureHangfireJobs();
			app.UseRouting();
			app.UseAuthorization();
			app.UseResponseCompression();
			app.UseGlobalExceptionHandler();
			app.UseEndpoints(endpoints =>
			{
				endpoints.MapHealthChecks("/healthz", new HealthCheckOptions());
				endpoints.MapControllers();
			});
		}
	}
}
