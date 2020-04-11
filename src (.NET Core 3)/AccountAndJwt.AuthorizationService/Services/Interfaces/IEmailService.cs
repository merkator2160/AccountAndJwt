using AccountAndJwt.AuthorizationService.Services.Models;
using System.Threading.Tasks;

namespace AccountAndJwt.AuthorizationService.Services.Interfaces
{
	public interface IEmailService
	{
		void Send(EmailMessage message);
		Task SendAsync(EmailMessage message);
	}
}