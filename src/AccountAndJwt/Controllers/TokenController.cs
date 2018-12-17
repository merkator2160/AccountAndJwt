using AccountAndJwt.Api.Middleware.Configs;
using AccountAndJwt.Api.Services.Interfaces;
using AccountAndJwt.Contracts.Models;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;

namespace AccountAndJwt.Api.Controllers
{
	/// <summary>
	/// Provide operationa under JWT access tokens for whole API
	/// </summary>
	[Route("api/[controller]/[action]")]
	public class TokenController : Controller
	{
		private readonly ITokenService _tokenService;
		private readonly ILogger<AccountController> _logger;
		private readonly IMapper _mapper;


		public TokenController(IOptions<AudienceConfig> audienceConfig, ITokenService tokenService, ILogger<AccountController> logger, IMapper mapper)
		{
			_tokenService = tokenService;
			_logger = logger;
			_mapper = mapper;
		}


		// ACTIONS ////////////////////////////////////////////////////////////////////////////////

		/// <summary>
		/// Get the access-token by username and password
		/// </summary>
		/// <param name="credentials"></param>
		/// <returns></returns>
		[HttpPost]
		[ProducesResponseType(typeof(RefreshTokenResponseAm), 200)]
		[ProducesResponseType(typeof(String), 400)]
		[ProducesResponseType(typeof(String), 500)]
		public IActionResult AuthorizeByCredentials([FromBody]AuthorizeByCredentialsRequestAm credentials)
		{
			try
			{
				if(!ModelState.IsValid)
					return BadRequest($"Please provide valid \"{nameof(credentials)}\"");

				var result = _tokenService.CreateAccessTokenByCredentials(credentials.Login, credentials.Password);
				_logger.LogInformation($"User authorized {credentials.Login}");

				return Ok(_mapper.Map<AuthorizeByCredentialsResponseAm>(result));
			}
			catch(ApplicationException ex)
			{
				return BadRequest(ex.Message);
			}
		}

		/// <summary>
		/// Get new access token by provided refresh token
		/// </summary>
		/// <param name="refreshToken"></param>
		/// <returns></returns>
		[HttpGet]
		[ProducesResponseType(typeof(AuthorizeByCredentialsResponseAm), 200)]
		[ProducesResponseType(typeof(String), 400)]
		[ProducesResponseType(typeof(String), 500)]
		public IActionResult RefreshToken(String refreshToken)
		{
			try
			{
				var result = _tokenService.CreateAccessTokenByRefreshToken(refreshToken);
				_logger.LogInformation("Token refreshed");

				return Ok(_mapper.Map<RefreshTokenResponseAm>(result));
			}
			catch(ApplicationException ex)
			{
				return BadRequest(ex.Message);
			}
		}

		/// <summary>
		/// Makes provided refresh token invalid
		/// </summary>
		/// <param name="refreshToken"></param>
		/// <returns></returns>
		[HttpPost]
		[ProducesResponseType(200)]
		[ProducesResponseType(typeof(String), 400)]
		[ProducesResponseType(typeof(String), 500)]
		public IActionResult RevokeToken(String refreshToken)
		{
			try
			{
				_tokenService.RevokeRefreshToken(refreshToken);
				_logger.LogInformation("Refresh token revoken");

				return Ok();
			}
			catch(ApplicationException ex)
			{
				return BadRequest(ex.Message);
			}
		}
	}
}