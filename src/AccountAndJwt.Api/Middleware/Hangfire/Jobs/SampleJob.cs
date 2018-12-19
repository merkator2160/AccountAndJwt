using AccountAndJwt.Api.Middleware.Hangfire.Interfaces;
using AccountAndJwt.Database.Interfaces;
using System;
using System.Threading.Tasks;

namespace AccountAndJwt.Api.Middleware.Hangfire.Jobs
{
	internal class SampleJob : IJob
	{
		private readonly IUnitOfWork _unitOfWork;


		public SampleJob(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}


		// IJob ///////////////////////////////////////////////////////////////////////////////////
		public async Task ExecuteAsync()
		{
			try
			{
				Console.WriteLine($"{nameof(SampleJob)} is executing");

				await Task.Delay(1000);
			}
			catch(Exception ex)
			{
				throw;
			}
		}
	}
}