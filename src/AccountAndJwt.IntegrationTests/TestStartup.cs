using AccountAndJwt.AuthorizationService.Middleware;
using AccountAndJwt.Common.Config;
using AccountAndJwt.Common.DependencyInjection;
using AccountAndJwt.Database;
using Autofac;
using Autofac.Extras.NLog;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
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
		private readonly IWebHostEnvironment _env;


		public TestStartup(IWebHostEnvironment env)
		{
			_env = env;
			_configuration = CustomConfigurationProvider.CreateConfiguration(env.EnvironmentName, env.WebRootPath);
		}


		// FUNCTIONS //////////////////////////////////////////////////////////////////////////////
		public void ConfigureServices(IServiceCollection services)
		{
			services.AddSingleton<IActionContextAccessor, ActionContextAccessor>()
				.AddScoped(x => x.GetRequiredService<IUrlHelperFactory>()
				.GetUrlHelper(x.GetRequiredService<IActionContextAccessor>().ActionContext));
			services.AddJwtAuthService(_configuration);
			services.AddHealthChecks();
			services.ConfigureResponseHandling();
			services
				.AddControllers()
				.AddApplicationPart(Assembly.Load(new AssemblyName("AccountAndJwt.AuthorizationService")))
				.AddNewtonsoftJson(options =>
				{
					options.SerializerSettings.ContractResolver = new DefaultContractResolver();
				})
				.SetCompatibilityVersion(CompatibilityVersion.Version_3_0);
		}
		public void ConfigureContainer(ContainerBuilder builder)
		{
			var authorizationServiceAssembly = Collector.GetAssembly("AccountAndJwt.AuthorizationService");
			builder.RegisterServiceConfiguration(_configuration, authorizationServiceAssembly);
			builder.RegisterServices(authorizationServiceAssembly);
			builder.RegisterModule<NLogModule>();
			builder.RegisterModule(new InMemoryDatabaseModule(_configuration, Collector.GetAssembly("AccountAndJwt.Database")));
			builder.RegisterModule(new AutoMapperModule(Collector.LoadAssemblies("AccountAndJwt")));
		}
		public void Configure(IApplicationBuilder app, IServiceProvider serviceProvider)
		{
			using(var context = serviceProvider.GetRequiredService<DataContext>())
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