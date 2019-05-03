using System;

namespace AccountAndJwt.AuthorizationService.Services.Models
{
	public class CreateAccessTokenByRefreshTokenDto
	{
		public String AccessToken { get; set; }
		public Double AccessTokenLifeTime { get; set; }
	}
}