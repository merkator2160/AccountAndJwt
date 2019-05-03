using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using NLog;
using NLog.Web;
using System;
using LogLevel = Microsoft.Extensions.Logging.LogLevel;

namespace AccountAndJwt.AuthorizationService
{
	public class Program
	{
		public static void Main(String[] args)
		{
			var logger = NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger();
			try
			{
				CreateWebHostBuilder(args).Build().Run();
			}
			catch(Exception ex)
			{
				logger.Error(ex, "Stopped program because of exception");
				throw;
			}
			finally
			{
				LogManager.Shutdown();
			}
		}

		public static IWebHostBuilder CreateWebHostBuilder(String[] args)
		{
			return WebHost.CreateDefaultBuilder(args)
				.UseStartup<Startup>()
				.ConfigureLogging((hostingContext, logging) =>
				{
					logging.ClearProviders();
					logging.SetMinimumLevel(LogLevel.Trace);
					logging.AddConfiguration(hostingContext.Configuration.GetSection("Logging"));
					logging.AddDebug();
				})
				.UseNLog();
		}
	}
}