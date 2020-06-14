using AccountAndJwt.AuthorizationService.Middleware;
using AccountAndJwt.AuthorizationService.Middleware.Cors;
using AccountAndJwt.AuthorizationService.Middleware.Hangfire;
using AccountAndJwt.Common.Config;
using AccountAndJwt.Common.DependencyInjection;
using AccountAndJwt.Database.DependencyInjection;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNet.OData.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json.Serialization;

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
			services.AddOData();
			services.AddControllers();
			services.AddMvcCore(options => { options.AddOdataMediaTypes(); });
			services
				.AddControllers()
				.AddNewtonsoftJson(options =>
				{
					options.SerializerSettings.ContractResolver = new DefaultContractResolver();
				})
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
				DatabaseModule.CheckDatabase(_configuration, DatabaseModule.InitializeStrategy);
			}
			if(_env.IsStaging())
			{
				app.UseDeveloperExceptionPage();
				app.UseCors(CorsPolicies.Staging);
				DatabaseModule.CheckDatabase(_configuration, DatabaseModule.InitializeStrategy);
			}
			if(_env.IsProduction())
			{
				app.UseCors(CorsPolicies.Production);
				DatabaseModule.CheckDatabase(_configuration, DatabaseModule.InitializeStrategy);
			}

			app.UseHsts();
			app.UseHttpsRedirection();
			app.UseRouting();
			app.UseAuthentication();
			app.UseAuthorization();
			app.UseHangfire();
			app.ConfigureHangfireJobs();
			app.UseResponseCompression();
			app.UseODataBatching();

			app.UseEndpoints(endpoints =>
			{
				endpoints.RegisterOdataRoutes(app);
				endpoints.MapHealthChecks("/healthz", new HealthCheckOptions());
				endpoints.MapControllers();
			});

			app.UseConfiguredSwagger();
			app.UseGlobalExceptionHandler();
		}
	}
}