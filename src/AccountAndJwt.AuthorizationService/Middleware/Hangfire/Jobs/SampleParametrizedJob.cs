using AccountAndJwt.AuthorizationService.Middleware.Hangfire.Models;
using AccountAndJwt.Database.Interfaces;
using DenverTraffic.Common.Hangfire.Interfaces;
using Hangfire;
using System;
using System.Threading.Tasks;

namespace AccountAndJwt.AuthorizationService.Middleware.Hangfire.Jobs
{
	internal class SampleParametrizedJob : IAsyncJob<SampleJobParameter>
	{
		private readonly IUnitOfWork _unitOfWork;


		public SampleParametrizedJob(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}


		// IJob ///////////////////////////////////////////////////////////////////////////////////
		[AutomaticRetry(Attempts = 0)]
		public async Task ExecuteAsync(SampleJobParameter parameter)
		{
			try
			{
				Console.WriteLine(parameter.Parameter);

				await Task.Delay(1000);
			}
			catch(Exception ex)
			{
				throw;
			}
		}
	}
}