using AccountAndJwt.Api.Database;
using AccountAndJwt.Api.Middleware;
using AccountAndJwt.Api.Middleware.AutoMapper;
using AccountAndJwt.Api.Middleware.Config;
using AccountAndJwt.Api.Middleware.Cors;
using AccountAndJwt.Api.Middleware.DependencyInjection;
using AccountAndJwt.Api.Middleware.Hangfire;
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
		public void ConfigureServices(IServiceCollection services)
		{
			services
				.AddSingleton<IActionContextAccessor, ActionContextAccessor>()
				.AddScoped(x => x.GetRequiredService<IUrlHelperFactory>().GetUrlHelper(x.GetRequiredService<IActionContextAccessor>().ActionContext));
			services.AddCors(CorsMiddleware.AddPolitics);
			services.AddConfiguredSwaggerGen();
			services.AddHangfire(_configuration);
			services.AddJwtAuthService(_configuration);
			services.AddMvc().AddJsonOptions(options => options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore);
		}
		private IServiceProvider BuildServiceProvider(IServiceCollection services)
		{
			var builder = new ContainerBuilder();

			builder.RegisterLocalServices();
			builder.RegisterLocalConfiguration(_configuration);

			builder.RegisterModule(new DatabaseModule(_configuration));
			builder.RegisterModule<AutoMapperModule>();

			builder.Populate(services);

			var container = builder.Build();
			container.RegisterJobActivator();

			return new AutofacServiceProvider(container);
		}
		public void Configure(IApplicationBuilder app, ILoggerFactory loggerFactory, DataContext dataContext)
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

			loggerFactory.AddConsole(_configuration.GetSection("Logging"));
			loggerFactory.AddDebug();

			dataContext.AddInitialData(_configuration["AudienceConfig:PasswordSalt"]);

			app.UseElmPage();
			app.UseElmCapture();

			if(_env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}

			app.UseHangfire();
			app.ConfigureHangfireJobs();
			app.UseAuthentication();
			app.UseConfiguredSwagger();
			app.UseGlobalExceptionHandler();
			app.UseAuthentication();
			app.UseMvcWithDefaultRoute();
		}
	}
}
