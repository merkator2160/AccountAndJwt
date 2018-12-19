using Microsoft.AspNetCore.Mvc;

namespace AccountAndJwt.Api.Controllers
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