using System;

namespace AccountAndJwt.AuthorizationService.Services.Models
{
	public class CreateAccessTokenByCredentialsDto
	{
		public String AccessToken { get; set; }
		public Int32 AccessTokenLifeTimeSec { get; set; }
		public String RefreshToken { get; set; }
	}
}