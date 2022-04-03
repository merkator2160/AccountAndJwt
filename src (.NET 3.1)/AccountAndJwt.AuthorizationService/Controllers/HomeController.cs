using Microsoft.AspNetCore.Mvc;
using System.Reflection;

namespace AccountAndJwt.AuthorizationService.Controllers
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
            return Ok($"Account and JWT v{Assembly.GetExecutingAssembly().GetName().Version}");
        }
    }
}