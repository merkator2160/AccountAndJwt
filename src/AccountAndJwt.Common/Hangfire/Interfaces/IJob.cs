using System.Threading.Tasks;

namespace Wialon.Common.Hangfire.Interfaces
{
	public interface IJob
	{
		Task ExecuteAsync();
	}
}