using Microsoft.AspNetCore.Mvc;

namespace AccountAndJwt.AuthorizationService.Controllers.Testing
{
	[ApiController]
#if !DEBUG
	[ApiExplorerSettings(IgnoreApi = true)]
#endif
	public class HomeController : ControllerBase
	{
		[HttpGet("/")]
		public IActionResult Index()
		{
			return Ok("Authorization service");
		}
	}
}