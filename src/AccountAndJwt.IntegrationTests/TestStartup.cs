using AccountAndJwt.Api.Database;
using AccountAndJwt.Api.Database.DependencyInjection;
using AccountAndJwt.Api.Middleware;
using AccountAndJwt.Api.Middleware.Config;
using AccountAndJwt.Api.Middleware.Cors;
using AccountAndJwt.Common.DependencyInjection;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;

namespace AccountAndJwt.IntegrationTests
{
	public class TestStartup
	{
		private readonly IConfiguration _configuration;
		private readonly IHostingEnvironment _env;


		public TestStartup(IHostingEnvironment env)
		{
			_env = env;
			_configuration = CustomConfigurationProvider.CreateConfiguration(env.EnvironmentName, env.WebRootPath);
		}


		// FUNCTIONS //////////////////////////////////////////////////////////////////////////////
		public IServiceProvider ConfigureServices(IServiceCollection services)
		{
			services.AddSingleton<IActionContextAccessor, ActionContextAccessor>()
				.AddScoped(x => x.GetRequiredService<IUrlHelperFactory>()
				.GetUrlHelper(x.GetRequiredService<IActionContextAccessor>().ActionContext));
			services.AddCors(CorsMiddleware.AddPolitics);
			services.AddConfiguredSwaggerGen();
			services.AddJwtAuthService(_configuration);
			services.AddMvc().AddJsonOptions(options => options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore);

			return BuildServiceProvider(services);
		}
		private IServiceProvider BuildServiceProvider(IServiceCollection services)
		{
			var builder = new ContainerBuilder();
			var pandaApiAssembly = Collector.GetAssembly("AccountAndJwt.Api");

			builder.RegisterServices(pandaApiAssembly);
			builder.RegisterConfiguration(_configuration, pandaApiAssembly);

			builder.RegisterModule(new InMemoryDatabaseModule(_configuration));
			builder.RegisterModule(new AutoMapperModule(Collector.LoadSolutionAssemblies()));

			builder.Populate(services);

			var container = builder.Build();

			return new AutofacServiceProvider(container);
		}
		public void Configure(IApplicationBuilder app, ILoggerFactory loggerFactory, IServiceProvider serviceProvider)
		{
			using(var context = serviceProvider.GetRequiredService<DataContext>())
			{
				context.Database.EnsureCreated();
				var salt = _configuration["AudienceConfig:PasswordSalt"];
				context.AddInitialData(salt);
			}

			app.UseDeveloperExceptionPage();
			app.UseCors(CorsPolicies.Development);

			loggerFactory.AddConsole(_configuration.GetSection("Logging"));
			loggerFactory.AddDebug();

			app.UseAuthentication();
			app.UseConfiguredSwagger();
			app.UseGlobalExceptionHandler();
			app.UseMvcWithDefaultRoute();
		}
	}
}