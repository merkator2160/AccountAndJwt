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
	[ApiController]
	[Route("api/[controller]/[action]")]
	[ApiExplorerSettings(IgnoreApi = true)]
	public class CoreTestController : ControllerBase
	{
		private readonly IServiceProvider _serviceProvider;


		public CoreTestController(IServiceProvider serviceProvider)
		{
			_serviceProvider = serviceProvider;
		}


		// ACTIONS ////////////////////////////////////////////////////////////////////////////////
		[HttpGet]
		[ProducesResponseType(200)]
		[ProducesResponseType(typeof(String), 500)]
		public IActionResult BasicTest()
		{
			return Ok();
		}

		[HttpGet]
		[ProducesResponseType(200)]
		[ProducesResponseType(typeof(String), 500)]
		public IActionResult ContextReferenceTest()
		{
			var contextInstance1 = _serviceProvider.GetRequiredService<DataContext>();
			var contextInstance2 = _serviceProvider.GetRequiredService<DataContext>();

			if(!Object.ReferenceEquals(contextInstance1, contextInstance2))
				throw new Exception("Data context are not the same!");

			return Ok();
		}

		[HttpGet]
		[ProducesResponseType(typeof(String), 500)]
		public IActionResult UnhandledExceptionTest()
		{
			throw new Exception("Exception message!");
		}

		[HttpGet]
		[ProducesResponseType(typeof(String), 500)]
		public IActionResult UnhandledApplicationExceptionTest()
		{
			throw new ApplicationException("ApplicationException message!");
		}
	}
}
#endif