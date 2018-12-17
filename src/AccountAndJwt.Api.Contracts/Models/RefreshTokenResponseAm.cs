using System;

namespace AccountAndJwt.Api.Contracts.Models
{
	/// <summary>
	/// Result of refreshing access token
	/// </summary>
	public class RefreshTokenResponseAm
	{
		/// <summary>
		/// New access token
		/// </summary>
		public String AccessToken { get; set; }

		/// <summary>
		/// New access token lifetime
		/// </summary>
		public Double AccessTokenLifeTime { get; set; }
	}
}