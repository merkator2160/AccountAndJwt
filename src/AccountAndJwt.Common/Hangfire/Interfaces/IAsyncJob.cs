using System.Threading.Tasks;

namespace DenverTraffic.Common.Hangfire.Interfaces
{
	public interface IAsyncJob
	{
		Task ExecuteAsync();
	}
}