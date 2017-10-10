using AccountAndJwt.Database;
using AccountAndJwt.Middleware;
using AccountAndJwt.Middleware.AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using NLog.Extensions.Logging;
using NLog.Web;
using System;

namespace AccountAndJwt
{
    internal class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);

            builder.AddEnvironmentVariables();
            Configuration = builder.Build();
            IsDevelopment = env.IsDevelopment();

            env.ConfigureNLog("nlog.config");
        }


        // PROPERTIES /////////////////////////////////////////////////////////////////////////////
        public IConfiguration Configuration { get; }
        public Boolean IsDevelopment { get; }


        // FUNCTIONS //////////////////////////////////////////////////////////////////////////////
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IActionContextAccessor, ActionContextAccessor>()
                .AddScoped(x => x.GetRequiredService<IUrlHelperFactory>()
                .GetUrlHelper(x.GetRequiredService<IActionContextAccessor>().ActionContext));
            services.AddConfigurations(Configuration);
            services.AddCustomServices();
            services.AddAutoMapperService();
            services.AddConfiguredSwaggerGen();
            services.AddDatabase(Configuration);
            services.AddJwtAuthService(Configuration);
            services.AddConfiguredElm(Configuration);
            services.AddMvc().AddJsonOptions(options => options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore);
        }
        public void Configure(IApplicationBuilder app, ILoggerFactory loggerFactory, DataContext dataContext)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();
            loggerFactory.AddNLog();

            dataContext.AddInitialData(Configuration["AudienceConfig:PasswordSalt"]);

            app.AddNLogWeb();
            app.UseElmPage();
            app.UseElmCapture();

            if (IsDevelopment)
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
