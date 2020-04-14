﻿using AccountAndJwt.AuthorizationService.Middleware;
using AccountAndJwt.Common.Config;
using AccountAndJwt.Common.DependencyInjection;
using AccountAndJwt.Database;
using Autofac;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

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
			services.AddMvc();
		}
		public void ConfigureContainer(ContainerBuilder builder)
		{
			var authorizationServiceAssembly = Collector.GetAssembly("AccountAndJwt.AuthorizationService");
			builder.RegisterConfiguration(_configuration, authorizationServiceAssembly);
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
				context.AddInitialData(salt);
			}

			app.UseRouting();
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