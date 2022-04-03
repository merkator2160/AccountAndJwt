using System;

namespace AccountAndJwt.AuthorizationService.Services.Models
{
	public class CreateAccessTokenByRefreshTokenDto
	{
		public String AccessToken { get; set; }
		public Int32 AccessTokenLifeTimeSec { get; set; }
	}
}