using System;

namespace AccountAndJwt.Api.Contracts.Models
{
	/// <summary>
	/// Result of authentication
	/// </summary>
	public class AuthorizeResponseAm
	{
		/// <summary>
		/// Access token itself
		/// </summary>
		public String AccessToken { get; set; }

		/// <summary>
		/// Access token lifetime
		/// </summary>
		public Double AccessTokenLifeTime { get; set; }

		/// <summary>
		/// Refresh token
		/// </summary>
		public String RefreshToken { get; set; }
	}
}