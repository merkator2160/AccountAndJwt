using AccountAndJwt.Database.Interfaces;
using DenverTraffic.Common.Hangfire.Interfaces;
using Hangfire;
using System;
using System.Threading.Tasks;

namespace AccountAndJwt.AuthorizationService.Middleware.Hangfire.Jobs
{
	internal class SampleJob : IAsyncJob
	{
		private readonly IUnitOfWork _unitOfWork;


		public SampleJob(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}


		// IJob ///////////////////////////////////////////////////////////////////////////////////
		[AutomaticRetry(Attempts = 0)]
		public async Task ExecuteAsync()
		{
			try
			{
				Console.WriteLine($"{nameof(SampleParametrizedJob)} is executing");

				await Task.Delay(1000);
			}
			catch(Exception ex)
			{
				throw;
			}
		}
	}
}