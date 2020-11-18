using System;

namespace AccountAndJwt.Contracts.Models
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
		public Int32 AccessTokenLifeTimeSec { get; set; }
	}
}