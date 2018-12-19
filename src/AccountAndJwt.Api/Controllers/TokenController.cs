using AccountAndJwt.Api.Contracts.Models;
using AccountAndJwt.Api.Services.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace AccountAndJwt.Api.Controllers
{
	/// <summary>
	/// Provide operations under JWT access tokens for whole API
	/// </summary>
	[Route("api/[controller]/[action]")]
	public class TokenController : Controller
	{
		private readonly ITokenService _tokenService;
		private readonly ILogger<AccountController> _logger;
		private readonly IMapper _mapper;


		public TokenController(ITokenService tokenService, ILogger<AccountController> logger, IMapper mapper)
		{
			_tokenService = tokenService;
			_logger = logger;
			_mapper = mapper;
		}


		// ACTIONS ////////////////////////////////////////////////////////////////////////////////

		/// <summary>
		/// Get the access-token by username and password
		/// </summary>
		[HttpPost]
		[ProducesResponseType(typeof(RefreshTokenResponseAm), 200)]
		[ProducesResponseType(typeof(String), 400)]
		[ProducesResponseType(typeof(String), 500)]
		public async Task<IActionResult> AuthorizeByCredentials([FromBody]AuthorizeRequestAm credentials)
		{
			try
			{
				if(!ModelState.IsValid)
					return BadRequest($"Please provide valid \"{nameof(credentials)}\"");

				var result = await _tokenService.CreateAccessTokenByCredentialsAsync(credentials.Login, credentials.Password);
				_logger.LogInformation($"User authorized {credentials.Login}");

				return Ok(_mapper.Map<AuthorizeResponseAm>(result));
			}
			catch(ApplicationException ex)
			{
				return BadRequest(ex.Message);
			}
		}

		/// <summary>
		/// Get new access token by provided refresh token
		/// </summary>
		[HttpPost]
		[ProducesResponseType(typeof(AuthorizeResponseAm), 200)]
		[ProducesResponseType(typeof(String), 400)]
		[ProducesResponseType(typeof(String), 500)]
		public async Task<IActionResult> RefreshToken([FromBody]String refreshToken)
		{
			try
			{
				var result = await _tokenService.CreateAccessTokenByRefreshTokenAsync(refreshToken);
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
		[HttpPost]
		[ProducesResponseType(200)]
		[ProducesResponseType(typeof(String), 400)]
		[ProducesResponseType(typeof(String), 500)]
		public async Task<IActionResult> RevokeToken([FromBody]String refreshToken)
		{
			try
			{
				await _tokenService.RevokeRefreshToken(refreshToken);
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