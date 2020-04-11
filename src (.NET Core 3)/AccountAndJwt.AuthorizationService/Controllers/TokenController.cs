using AccountAndJwt.AuthorizationService.Services.Interfaces;
using AccountAndJwt.Contracts.Models;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace AccountAndJwt.AuthorizationService.Controllers
{
	/// <summary>
	/// Provide operations under JWT access tokens for whole API
	/// </summary>
	[ApiController]
	[Route("api/[controller]/[action]")]
	public class TokenController : ControllerBase
	{
		private readonly ITokenService _tokenService;
		private readonly IMapper _mapper;


		public TokenController(ITokenService tokenService, IMapper mapper)
		{
			_tokenService = tokenService;
			_mapper = mapper;
		}


		// ACTIONS ////////////////////////////////////////////////////////////////////////////////

		/// <summary>
		/// Get the access-token by username and password
		/// </summary>
		[HttpPost]
		[ProducesResponseType(typeof(AuthorizeResponseAm), 200)]
		[ProducesResponseType(typeof(String), 460)]
		[ProducesResponseType(typeof(String), 500)]
		public async Task<IActionResult> AuthorizeByCredentials([FromBody]AuthorizeRequestAm credentials)
		{
			if(!ModelState.IsValid)
				return StatusCode(460, $"Please provide valid \"{nameof(credentials)}\"");

			var result = await _tokenService.CreateAccessTokenByCredentialsAsync(credentials.Login, credentials.Password);

			return Ok(_mapper.Map<AuthorizeResponseAm>(result));
		}

		/// <summary>
		/// Get new access token by provided refresh token
		/// </summary>
		[HttpPost]
		[ProducesResponseType(typeof(RefreshTokenResponseAm), 200)]
		[ProducesResponseType(typeof(String), 460)]
		[ProducesResponseType(typeof(String), 500)]
		public async Task<IActionResult> RefreshToken([FromBody]String refreshToken)
		{
			var result = await _tokenService.CreateAccessTokenByRefreshTokenAsync(refreshToken);

			return Ok(_mapper.Map<RefreshTokenResponseAm>(result));
		}

		/// <summary>
		/// Makes provided refresh token invalid
		/// </summary>
		[HttpPost]
		[ProducesResponseType(200)]
		[ProducesResponseType(typeof(String), 460)]
		[ProducesResponseType(typeof(String), 500)]
		public async Task<IActionResult> RevokeToken([FromBody]String refreshToken)
		{
			await _tokenService.RevokeRefreshToken(refreshToken);

			return Ok();
		}
	}
}