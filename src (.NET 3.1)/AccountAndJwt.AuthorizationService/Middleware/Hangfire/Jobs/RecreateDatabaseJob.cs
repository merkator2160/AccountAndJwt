using AccountAndJwt.Common.Hangfire.Interfaces;
using AccountAndJwt.Database.DependencyInjection;
using Hangfire;
using Microsoft.Extensions.Configuration;
using NLog;
using System;
using System.Threading;

namespace AccountAndJwt.AuthorizationService.Middleware.Hangfire.Jobs
{
	internal class RecreateDatabaseJob : IJob, IDisposable
	{
		private readonly IConfiguration _configuration;
		private readonly ILogger _logger;

		private readonly Boolean _isMutexFree;
		private readonly Mutex _mutex;


		public RecreateDatabaseJob(IConfiguration configuration, ILogger logger)
		{
			_configuration = configuration;
			_logger = logger;

			_mutex = new Mutex(true, nameof(RecreateDatabaseJob), out _isMutexFree);
		}


		// IJob ///////////////////////////////////////////////////////////////////////////////////
		[AutomaticRetry(Attempts = 0)]
		public void Execute()
		{
			try
			{
				if(!_isMutexFree)
					return;

				DatabaseModule.CheckDatabase(_configuration, DatabaseModule.DropCreateInitializeStrategy);
			}
			catch(Exception ex)
			{
				_logger.Error(ex, $"{ex.Message}\r\n{ex.StackTrace}");
				throw;
			}
		}


		// IDisposable ////////////////////////////////////////////////////////////////////////////
		public void Dispose()
		{
			_mutex?.Dispose();
		}
	}
}