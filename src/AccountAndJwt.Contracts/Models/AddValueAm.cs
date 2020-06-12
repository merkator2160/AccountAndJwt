using System;
using System.ComponentModel.DataAnnotations;

namespace AccountAndJwt.Contracts.Models
{
	public class AddValueAm
	{
		/// <summary>
		/// Value data
		/// </summary>
		[Required]
		public Int32 Value { get; set; }

		/// <summary>
		/// Value commentary
		/// </summary>
		public String Commentary { get; set; }
	}
}