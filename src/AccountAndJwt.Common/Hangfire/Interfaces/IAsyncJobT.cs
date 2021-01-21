using System.Threading.Tasks;

namespace DenverTraffic.Common.Hangfire.Interfaces
{
	public interface IAsyncJob<T>
	{
		Task ExecuteAsync(T parameter);
	}
}