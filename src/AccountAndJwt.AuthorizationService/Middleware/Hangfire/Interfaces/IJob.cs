using System.Threading.Tasks;

namespace AccountAndJwt.AuthorizationService.Middleware.Hangfire.Interfaces
{
	internal interface IJob
	{
		Task ExecuteAsync();
	}
}