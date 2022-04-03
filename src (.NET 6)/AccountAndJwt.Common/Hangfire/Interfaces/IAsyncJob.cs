using System.Threading.Tasks;

namespace AccountAndJwt.Common.Hangfire.Interfaces
{
	public interface IAsyncJob
	{
		Task ExecuteAsync();
	}
}