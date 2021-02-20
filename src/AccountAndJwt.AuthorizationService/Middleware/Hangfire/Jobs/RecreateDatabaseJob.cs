using AccountAndJwt.Database.DependencyInjection;
using DenverTraffic.Common.Hangfire.Interfaces;
using Hangfire;
using Microsoft.Extensions.Configuration;

namespace AccountAndJwt.AuthorizationService.Middleware.Hangfire.Jobs
{
	internal class RecreateDatabaseJob : IJob
	{
		private readonly IConfiguration _configuration;


		public RecreateDatabaseJob(IConfiguration configuration)
		{
			_configuration = configuration;
		}


		// IJob ///////////////////////////////////////////////////////////////////////////////////
		[AutomaticRetry(Attempts = 0)]
		public void Execute()
		{
			DatabaseModule.CheckDatabase(_configuration, DatabaseModule.DropCreateInitializeStrategy);
		}
	}
}