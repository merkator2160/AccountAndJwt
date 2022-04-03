using AccountAndJwt.AuthorizationService.Middleware.Hangfire.Models;
using AccountAndJwt.Common.Hangfire.Interfaces;
using Hangfire;
using NLog;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace AccountAndJwt.AuthorizationService.Middleware.Hangfire.Jobs
{
	internal class SampleAsyncParametrizedJob : IAsyncJob<SampleJobParameter>, IDisposable
	{
		private readonly ILogger _logger;

		private readonly Boolean _isMutexFree;
		private readonly Mutex _mutex;


		public SampleAsyncParametrizedJob(ILogger logger)
		{
			_logger = logger;

			_mutex = new Mutex(true, nameof(SampleAsyncParametrizedJob), out _isMutexFree);
		}


		// IJob ///////////////////////////////////////////////////////////////////////////////////
		[AutomaticRetry(Attempts = 0)]
		public async Task ExecuteAsync(SampleJobParameter parameter)
		{
			try
			{
				if(!_isMutexFree)
					return;

				Console.WriteLine($"{nameof(SampleAsyncParametrizedJob)} is executing, parameter: {parameter.Parameter}");

				await Task.Delay(1000);
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