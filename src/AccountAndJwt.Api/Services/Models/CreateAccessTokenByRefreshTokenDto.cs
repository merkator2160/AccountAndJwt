using System;

namespace AccountAndJwt.Api.Services.Models
{
	public class CreateAccessTokenByRefreshTokenDto
	{
		public String AccessToken { get; set; }
		public Double AccessTokenLifeTime { get; set; }
	}
}