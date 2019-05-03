using System;
using System.ComponentModel.DataAnnotations;

namespace AccountAndJwt.Contracts.Models
{
	/// <summary>
	/// Value model
	/// </summary>
	public class ValueAm
	{
		/// <summary>
		/// Value unique identifyer
		/// </summary>
		[Required]
		public Int32 Id { get; set; }

		/// <summary>
		/// Value data
		/// </summary>
		[Required]
		public String Value { get; set; }
	}
}