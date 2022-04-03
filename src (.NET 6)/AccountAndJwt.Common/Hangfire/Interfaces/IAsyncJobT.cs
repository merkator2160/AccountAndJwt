using System.Threading.Tasks;

namespace AccountAndJwt.Common.Hangfire.Interfaces
{
	public interface IAsyncJob<T>
	{
		Task ExecuteAsync(T parameter);
	}
}