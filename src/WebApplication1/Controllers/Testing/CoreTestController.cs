using AccountAndJwt.Database;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System;

#if DEBUG
namespace AccountAndJwt.AuthorizationService.Controllers.Testing
{
	/// <summary>
	/// This controller only for integration testing purposes (it does not appear in production)
	/// </summary>
	[ApiExplorerSettings(IgnoreApi = true)]
	[Route("api/[controller]/[action]")]
	public class CoreTestController : Controller
	{
		private readonly IServiceProvider _serviceProvider;


		public CoreTestController(IServiceProvider serviceProvider)
		{
			_serviceProvider = serviceProvider;
		}


		// ACTIONS ////////////////////////////////////////////////////////////////////////////////
		[HttpGet]
		public IActionResult BasicTest()
		{
			return Ok();
		}

		[HttpGet]
		public IActionResult ContextReferenceTest()
		{
			var contextInstance1 = _serviceProvider.GetRequiredService<DataContext>();
			var contextInstance2 = _serviceProvider.GetRequiredService<DataContext>();

			if(!Object.ReferenceEquals(contextInstance1, contextInstance2))
				throw new Exception("Data context are not the same!");

			return Ok();
		}
	}
}
#endif