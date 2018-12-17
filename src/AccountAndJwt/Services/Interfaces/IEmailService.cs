using AccountAndJwt.Api.Services.Models;
using System.Threading.Tasks;

namespace AccountAndJwt.Api.Services.Interfaces
{
	public interface IEmailService
	{
		void Send(EmailMessage message);
		Task SendAsync(EmailMessage message);
	}
}