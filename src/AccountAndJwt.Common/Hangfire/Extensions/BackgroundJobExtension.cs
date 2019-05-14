using Hangfire;
using Wialon.Common.Hangfire.Interfaces;

namespace Wialon.Common.Hangfire.Extensions
{
	public static class BackgroundJobExtension
	{
		public static void Enqueue<T>(this BackgroundJob job) where T : IJob
		{
			BackgroundJob.Enqueue<T>(p => p.ExecuteAsync());
		}
	}
}