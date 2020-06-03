using System.Threading.Tasks;

namespace AccountAndJwt.Common.Hangfire.Interfaces
{
	public interface IJob<T>
	{
		Task ExecuteAsync(T parameter);
	}
}