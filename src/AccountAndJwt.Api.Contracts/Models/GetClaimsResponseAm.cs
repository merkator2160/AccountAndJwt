using System;

namespace AccountAndJwt.Api.Contracts.Models
{
	/// <summary>
	/// Get current user claims response model
	/// </summary>
	public class GetClaimsResponseAm
	{
		/// <summary>
		/// Type of the claim
		/// </summary>
		public String ClaimType { get; set; }

		/// <summary>
		/// Claim value
		/// </summary>
		public String Value { get; set; }
	}
}