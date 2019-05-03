using Microsoft.AspNetCore.Mvc;

namespace AccountAndJwt.AuthorizationService.Controllers
{
	[ApiExplorerSettings(IgnoreApi = true)]
	public class HomeController : Controller
	{
		[HttpGet("/")]
		public IActionResult Index()
		{
			return Ok();
		}
	}
}