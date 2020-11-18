using AccountAndJwt.Database;
using AccountAndJwt.Database.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;

#if DEBUG
namespace AccountAndJwt.AuthorizationService.Controllers.Testing
{
	/// <summary>
	/// This controller only for debugging, playing, testing purposes (it does not appear in production)
	/// </summary>
	[ApiController]
#if !DEVELOPMENT
	[ApiExplorerSettings(IgnoreApi = false)]
#endif
	[Route("api/[controller]/[action]")]
	public class DebugController : ControllerBase
	{
		private readonly IWebHostEnvironment _env;
		private readonly ILogger<DebugController> _logger;
		private readonly IMapper _mapper;
		private readonly IUnitOfWork _unitOfWork;
		private readonly DataContext _context;


		public DebugController(
			IWebHostEnvironment env,
			ILogger<DebugController> logger,
			IMapper mapper,
			IUnitOfWork unitOfWork,
			DataContext context)
		{
			_env = env;
			_logger = logger;
			_mapper = mapper;
			_unitOfWork = unitOfWork;
			_context = context;
		}


		// ACTIONS //////////////////////////////////////////////////////////////////////////////////////

		/// <summary>
		/// Creates the log entry
		/// </summary>
		/// <returns></returns>
		[HttpPost]
		[ProducesResponseType(200)]
		[ProducesResponseType(typeof(String), 500)]
		public IActionResult WriteLog(String message)
		{
			_logger.LogError($"Test error log: {message}");
			_logger.LogDebug($"Test debug log: {message}");

			return Ok();
		}

		/// <summary>
		/// Returns current environment name
		/// </summary>
		[HttpGet]
		[ProducesResponseType(200)]
		[ProducesResponseType(typeof(String), 500)]
		public IActionResult GetEnvironmentName()
		{
			return Ok(_env.EnvironmentName);
		}

		/// <summary>
		/// Returns a variable associated with current environment
		/// </summary>
		[HttpGet]
		[ProducesResponseType(typeof(String), 200)]
		[ProducesResponseType(typeof(String), 500)]
		public IActionResult GetEnvironmentVariable()
		{
			return Ok(Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT"));
		}

		/// <summary>
		/// Returns information about available repositories
		/// </summary>
		[HttpGet]
		[ProducesResponseType(typeof(String), 200)]
		[ProducesResponseType(typeof(String), 500)]
		public IActionResult GetAvailableRepositories()
		{
			return Ok(_unitOfWork);
		}
	}
}
#endif