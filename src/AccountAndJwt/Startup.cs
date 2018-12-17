using AccountAndJwt.Api.Database;
using AccountAndJwt.Api.Middleware;
using AccountAndJwt.Api.Middleware.AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using NLog.Extensions.Logging;

namespace AccountAndJwt.Api
{
	internal class Startup
	{
		private readonly IConfiguration _configuration;
		private readonly IHostingEnvironment _env;


		public Startup(IHostingEnvironment env)
		{
			var builder = new ConfigurationBuilder()
				.SetBasePath(env.ContentRootPath)
				.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
				.AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);

			builder.AddEnvironmentVariables();

			_configuration = builder.Build();
			_env = env;
		}


		// FUNCTIONS //////////////////////////////////////////////////////////////////////////////
		public void ConfigureServices(IServiceCollection services)
		{
			services.AddSingleton<IActionContextAccessor, ActionContextAccessor>()
				.AddScoped(x => x.GetRequiredService<IUrlHelperFactory>()
				.GetUrlHelper(x.GetRequiredService<IActionContextAccessor>().ActionContext));
			services.AddConfigurations(_configuration);
			services.AddCustomServices();
			services.AddAutoMapperService();
			services.AddConfiguredSwaggerGen();
			services.AddDatabase(_configuration);
			services.AddJwtAuthService(_configuration);
			services.AddConfiguredElm(_configuration);
			services.AddMvc().AddJsonOptions(options => options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore);
		}
		public void Configure(IApplicationBuilder app, ILoggerFactory loggerFactory, DataContext dataContext)
		{
			loggerFactory.AddConsole(_configuration.GetSection("Logging"));
			loggerFactory.AddDebug();
			loggerFactory.AddNLog();

			dataContext.AddInitialData(_configuration["AudienceConfig:PasswordSalt"]);

			app.UseElmPage();
			app.UseElmCapture();

			if(_env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}

			app.UseConfiguredSwagger();
			app.UseGlobalExceptionHandler();
			app.UseAuthentication();
			app.UseMvcWithDefaultRoute();
		}
	}
}
