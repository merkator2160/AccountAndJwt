using AccountAndJwt.Api.Middleware;
using AccountAndJwt.Api.Middleware.Config;
using AccountAndJwt.Api.Middleware.Cors;
using AccountAndJwt.Api.Middleware.Hangfire;
using AccountAndJwt.Common.DependencyInjection;
using AccountAndJwt.Database.DependencyInjection;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;

namespace AccountAndJwt.Api
{
	internal class Startup
	{
		private readonly IConfiguration _configuration;
		private readonly IHostingEnvironment _env;


		public Startup(IHostingEnvironment env)
		{
			_env = env;
			_configuration = CustomConfigurationProvider.CreateConfiguration(env.EnvironmentName, env.ContentRootPath);
		}


		// FUNCTIONS //////////////////////////////////////////////////////////////////////////////
		public IServiceProvider ConfigureServices(IServiceCollection services)
		{
			services.AddCors(CorsMiddleware.AddPolitics);
			services.AddConfiguredSwaggerGen();
			services.AddHangfire(_configuration);
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

			builder.RegisterLocalServices();
			builder.RegisterLocalConfiguration(_configuration);

			builder.RegisterModule(new DatabaseModule(_configuration));
			builder.RegisterModule(new AutoMapperModule(Collector.LoadAssemblies("AccountAndJwt")));

			builder.Populate(services);

			var container = builder.Build();
			container.RegisterJobActivator();

			return new AutofacServiceProvider(container);
		}
		public void Configure(IApplicationBuilder app)
		{
			if(_env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
				app.UseCors(CorsPolicies.Development);
				DatabaseModule.CreateDatabase(_configuration, DatabaseModule.InitializeStrategy);
			}
			if(_env.IsStaging())
			{
				app.UseDeveloperExceptionPage();
				app.UseCors(CorsPolicies.Staging);
				DatabaseModule.CreateDatabase(_configuration, DatabaseModule.InitializeStrategy);
			}
			if(_env.IsProduction())
			{
				app.UseCors(CorsPolicies.Production);
				DatabaseModule.CreateDatabase(_configuration, DatabaseModule.InitializeStrategy);
			}

			app.UseHangfire();
			app.ConfigureHangfireJobs();
			app.UseConfiguredSwagger();
			app.UseGlobalExceptionHandler();
			app.UseAuthentication();
			app.UseMvcWithDefaultRoute();
		}
	}
}
