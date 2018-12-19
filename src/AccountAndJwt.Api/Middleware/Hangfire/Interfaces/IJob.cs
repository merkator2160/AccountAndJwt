using System.Threading.Tasks;

namespace AccountAndJwt.Api.Middleware.Hangfire.Interfaces
{
	internal interface IJob
	{
		Task ExecuteAsync();
	}
}