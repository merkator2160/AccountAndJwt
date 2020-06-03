using AccountAndJwt.AuthorizationService.Middleware.Hangfire.Models;
using AccountAndJwt.Common.Hangfire.Interfaces;
using AccountAndJwt.Database.Interfaces;
using Hangfire;
using System;
using System.Threading.Tasks;

namespace AccountAndJwt.AuthorizationService.Middleware.Hangfire.Jobs
{
	internal class SampleParametrizedJob : IJob<SampleJobParameter>
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