using AccountAndJwt.AuthorizationService.Middleware.Hangfire.Auth;
using AccountAndJwt.AuthorizationService.Middleware.Hangfire.Jobs;
using AccountAndJwt.Database.DependencyInjection;
using Autofac;
using Hangfire;
using Hangfire.SqlServer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Globalization;

namespace AccountAndJwt.AuthorizationService.Middleware.Hangfire
{
	/// <summary>
	/// https://stackoverflow.com/questions/44383174/hangfire-with-horizontal-scaling
	/// https://stackoverflow.com/questions/42201809/hangfire-recurring-job-on-every-server/42202844
	/// http://docs.hangfire.io/en/latest/background-processing/configuring-queues.html
	/// </summary>
	internal static class HangfireMiddleware
	{
		public static void UseHangfire(this IApplicationBuilder app)
		{
			app.UseHangfireDashboard("/hangfire", new DashboardOptions()
			{
				Authorization = new[]
				{
					new FreeAuthorizationFilter()
				}
			});
			app.UseHangfireServer(new BackgroundJobServerOptions()
			{
				Queues = new[] { CreateEnvironmentDependentQueueName() }
			});
		}
		public static void AddHangfire(this IServiceCollection services, IConfiguration configuration)
		{
			var databaseModule = new DatabaseModule(configuration);
			services.AddHangfire(config =>
			{
				config.UseSqlServerStorage(databaseModule.ConnectionString, new SqlServerStorageOptions()
				{
					DashboardJobListLimit = 1000,
					CommandTimeout = TimeSpan.FromSeconds(databaseModule.Config.CommandTimeout),
					TransactionTimeout = TimeSpan.FromSeconds(databaseModule.Config.CommandTimeout),
					CommandBatchMaxTimeout = TimeSpan.FromSeconds(databaseModule.Config.CommandTimeout)
				});
			});
		}
		public static void RegisterJobActivator(this IContainer container)
		{
			GlobalConfiguration.Configuration.UseAutofacActivator(container);
		}
		public static void ConfigureHangfireJobs(this IApplicationBuilder app)
		{
			RecurringJob.AddOrUpdate<SampleJob>(
				p => p.ExecuteAsync(),
				Cron.Minutely,
				timeZone: TimeZoneInfo.Utc,
				queue: CreateEnvironmentDependentQueueName());
		}


		// SUPPORT FUNCTIONS //////////////////////////////////////////////////////////////////////
		private static String CreateEnvironmentDependentQueueName()
		{
			return Environment.MachineName.Replace("-", "").ToLower(CultureInfo.InvariantCulture);
		}
	}
}