using AccountAndJwt.AuthorizationService.Middleware.Hangfire.Jobs;
using AccountAndJwt.AuthorizationService.Middleware.Hangfire.Models;
using AccountAndJwt.Common.Hangfire.Auth;
using AccountAndJwt.Database.DependencyInjection;
using AccountAndJwt.Database.Models.Config;
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
				Queues = new[] { "default", CreateEnvironmentDependentQueueName() }
			});
		}
		public static void AddHangfire(this IServiceCollection services, IConfiguration configuration)
		{
			var connectionString = configuration.GetConnectionString(DatabaseModule.DefaultConnectionStringName);
			var databaseConfig = configuration.GetSection("DatabaseConfig").Get<DatabaseConfig>();

			var databaseModule = new DatabaseModule(configuration);
			services.AddHangfire(config =>
			{
				config.UseSqlServerStorage(connectionString, new SqlServerStorageOptions()
				{
					DashboardJobListLimit = 1000,
					CommandTimeout = TimeSpan.FromSeconds(databaseConfig.CommandTimeout),
					TransactionTimeout = TimeSpan.FromSeconds(databaseConfig.CommandTimeout),
					CommandBatchMaxTimeout = TimeSpan.FromSeconds(databaseConfig.CommandTimeout)
				});
				//or
				//config.UseMemoryStorage();
			});
		}
		public static void RegisterJobActivator(this ILifetimeScope scope)
		{
			GlobalConfiguration.Configuration.UseAutofacActivator(scope);
		}
		public static void ConfigureHangfireJobs(this IApplicationBuilder app)
		{
#if DEVELOPMENT
			var parameter = new SampleJobParameter()
			{
				Parameter = $"{nameof(SampleParametrizedJob)} is executing"
			};

			BackgroundJob.Enqueue<SampleParametrizedJob>(p => p.ExecuteAsync(parameter));
			RecurringJob.AddOrUpdate<SampleParametrizedJob>(
				p => p.ExecuteAsync(parameter),
				Cron.Minutely,
				timeZone: TimeZoneInfo.Utc,
				queue: CreateEnvironmentDependentQueueName());
#else
			ConfigureOneTimeJobs();
			ConfigureRecurringJobs();
#endif
		}


		// SUPPORT FUNCTIONS //////////////////////////////////////////////////////////////////////
		private static String CreateEnvironmentDependentQueueName()
		{
			return Environment.MachineName.Replace("-", "").ToLower(CultureInfo.InvariantCulture);
		}
		private static void ConfigureOneTimeJobs()
		{
			//BackgroundJob.Enqueue<DeliveryReportJob>(p => p.ExecuteAsync());
		}
		private static void ConfigureRecurringJobs()
		{
			//RecurringJob.AddOrUpdate<SampleJob>(
			// p => p.ExecuteAsync(),
			// Cron.Minutely,
			// timeZone: TimeZoneInfo.Utc,
			// queue: CreateEnvironmentDependentQueueName());
		}
	}
}