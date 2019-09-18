﻿using AccountAndJwt.AuthorizationService.Middleware;
using AccountAndJwt.Common.Config;
using AccountAndJwt.Common.DependencyInjection;
using AccountAndJwt.Database;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
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
			services.AddJwtAuthService(_configuration);
			services
				.AddMvc()
				.AddJsonOptions(options => options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore)
				.SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

			return BuildServiceProvider(services);
		}
		private IServiceProvider BuildServiceProvider(IServiceCollection services)
		{
			var builder = new ContainerBuilder();

			var authorizationServiceAssembly = Collector.GetAssembly("AccountAndJwt.AuthorizationService");
			builder.RegisterConfiguration(_configuration, authorizationServiceAssembly);
			builder.RegisterServices(authorizationServiceAssembly);

			builder.RegisterModule(new InMemoryDatabaseModule(_configuration, Collector.GetAssembly("AccountAndJwt.Database")));
			builder.RegisterModule(new AutoMapperModule(Collector.LoadAssemblies("AccountAndJwt")));

			builder.Populate(services);

			var container = builder.Build();

			return new AutofacServiceProvider(container);
		}
		public void Configure(IApplicationBuilder app, IServiceProvider serviceProvider)
		{
			using(var context = serviceProvider.GetRequiredService<DataContext>())
			{
				context.Database.EnsureCreated();
				var salt = _configuration["AudienceConfig:PasswordSalt"];
				context.AddInitialData(salt);
			}

			app.UseDeveloperExceptionPage();

			app.UseAuthentication();
			app.UseGlobalExceptionHandler();
			app.UseMvcWithDefaultRoute();
		}
	}
}