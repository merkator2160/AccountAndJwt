using AccountAndJwt.AuthorizationService.Middleware.Hangfire.Jobs;
using AccountAndJwt.AuthorizationService.Middleware.Hangfire.Models;
using AccountAndJwt.Common.Hangfire.Auth;
using AccountAndJwt.Database.DependencyInjection;
using AccountAndJwt.Database.Models.Config;
using Autofac;
using Hangfire;
using Hangfire.SqlServer;
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
        }
        public static void AddHangfire(this IServiceCollection services, IConfiguration configuration)
        {
            //var connectionString = configuration.GetConnectionString(DatabaseModule.ConnectionStringName);    // Extracts connection string from environment configuration file
            var connectionString = configuration.GetSection(DatabaseModule.ConnectionStringName).Value;        // Extracts connection string from environment variable
            var databaseConfig = configuration.GetSection("DatabaseConfig").Get<DatabaseConfig>();

            services.AddHangfireServer(options =>
            {
                options.Queues = new[] { "default" };
            });
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
#if DEBUG
            var parameter = new SampleJobParameter()
            {
                Parameter = $"{nameof(SampleAsyncParametrizedJob)} is executing"
            };

            BackgroundJob.Enqueue<SampleAsyncParametrizedJob>(p => p.ExecuteAsync(parameter));
            RecurringJob.AddOrUpdate<SampleAsyncParametrizedJob>(
                p => p.ExecuteAsync(parameter),
                Cron.Never,
                TimeZoneInfo.Utc,
                CreateEnvironmentDependentQueueName());

            RecurringJob.AddOrUpdate<RecreateDatabaseJob>(
                p => p.Execute(),
                Cron.Never(),
                TimeZoneInfo.Utc);
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
            BackgroundJob.Enqueue<SampleAsyncJob>(p => p.ExecuteAsync());
            RecurringJob.AddOrUpdate<RecreateDatabaseJob>(
                p => p.Execute(),
                Cron.Never(),
                TimeZoneInfo.Utc);
        }
        private static void ConfigureRecurringJobs()
        {
            RecurringJob.AddOrUpdate<SampleAsyncJob>(
             p => p.ExecuteAsync(),
             Cron.Minutely,
             timeZone: TimeZoneInfo.Utc,
             queue: CreateEnvironmentDependentQueueName());
        }
    }
}