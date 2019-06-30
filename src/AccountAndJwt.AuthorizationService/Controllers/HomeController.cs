using Microsoft.AspNetCore.Mvc;

namespace AccountAndJwt.AuthorizationService.Controllers
{
	[ApiController]
	[ApiExplorerSettings(IgnoreApi = true)]
	public class HomeController : ControllerBase
	{
		[HttpGet("/")]
		public IActionResult Index()
		{
			return Ok();
		}
	}
}