using AccountAndJwt.Api.Contracts.Models;
using AccountAndJwt.Database.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;

#if DEBUG
namespace AccountAndJwt.Api.Controllers.Testing
{
	/// <summary>
	/// This controller only for debugging, playing, testing purposes (it does not appear in production)
	/// </summary>
	[Route("api/[controller]/[action]")]
	public class DebugController : Controller
	{
		private readonly IHostingEnvironment _env;
		private readonly ILogger<DebugController> _logger;
		private readonly IMapper _mapper;
		private readonly IUnitOfWork _unitOfWork;


		public DebugController(
			IHostingEnvironment env,
			ILogger<DebugController> logger,
			IMapper mapper,
			IUnitOfWork unitOfWork)
		{
			_env = env;
			_logger = logger;
			_mapper = mapper;
			_unitOfWork = unitOfWork;
		}


		// ACTIONS //////////////////////////////////////////////////////////////////////////////////////
		/// <summary>
		/// [Auth] Get current user associated data 
		/// </summary>
		/// <returns></returns>
		[Authorize]
		[HttpGet]
		[ProducesResponseType(typeof(GetClaimsResponseAm[]), 200)]
		[ProducesResponseType(401)]
		[ProducesResponseType(typeof(String), 500)]
		public IActionResult GetCurrentUserClaims()
		{
			var claims = HttpContext.User.Claims.ToArray().Select(x => new GetClaimsResponseAm
			{
				ClaimType = x.Type,
				Value = x.Value
			});
			return Ok(claims);
		}

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

		[HttpGet]
		[ProducesResponseType(200)]
		[ProducesResponseType(typeof(String), 500)]
		public IActionResult GetEnvironmentName()
		{
			return Ok(_env.EnvironmentName);
		}

		/// <summary>
		/// Sample action implementation to test something in the future.
		/// </summary>
		[HttpGet]
		[ProducesResponseType(typeof(String), 200)]
		[ProducesResponseType(typeof(String), 500)]
		public IActionResult TestAction()
		{
			try
			{
				return Ok(_unitOfWork);
			}
			catch(ApplicationException ex)
			{
				return BadRequest(ex.Message);
			}
		}
	}
}
#endif