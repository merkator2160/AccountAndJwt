using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog;
using NLog.Web;
using System;
using LogLevel = Microsoft.Extensions.Logging.LogLevel;

namespace AccountAndJwt.AuthorizationService
{
	internal class Program
	{
		private static void Main(String[] args)
		{
			var logger = NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger();
			try
			{
				CreateHostBuilder(args).Build().Run();
			}
			catch(Exception ex)
			{
				logger.Error(ex, $"{ex.Message}\r\n{ex.StackTrace}");
				throw;
			}
			finally
			{
				LogManager.Shutdown();
			}
		}
		private static IHostBuilder CreateHostBuilder(String[] args)
		{
			return Host.CreateDefaultBuilder(args)
				.UseServiceProviderFactory(new AutofacServiceProviderFactory())
				.ConfigureWebHostDefaults(webBuilder =>
				{
					webBuilder.UseStartup<Startup>();
#if !DEVELOPMENT
					// For standalone launch configurations
					//webBuilder.UseKestrel();
					//webBuilder.UseUrls("http://*:5001");
#endif
					webBuilder
						.ConfigureLogging((hostingContext, logging) =>
						{
							logging.ClearProviders();
							logging.SetMinimumLevel(LogLevel.Trace);
							logging.AddConfiguration(hostingContext.Configuration.GetSection("Logging"));
							logging.AddDebug();
						})
						.UseNLog();
				});
		}
	}
}
