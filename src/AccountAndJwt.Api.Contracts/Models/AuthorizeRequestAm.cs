using System;
using System.ComponentModel.DataAnnotations;

namespace AccountAndJwt.Api.Contracts.Models
{
	/// <summary>
	/// Get JWT token by credentials model
	/// </summary>
	public class AuthorizeRequestAm
	{
		/// <summary>
		/// User name
		/// </summary>
		[Required]
		public String Login { get; set; }

		/// <summary>
		/// Password
		/// </summary>
		[Required]
		public String Password { get; set; }
	}
}