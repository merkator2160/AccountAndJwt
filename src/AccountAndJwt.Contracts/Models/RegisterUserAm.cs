﻿using System;
using System.ComponentModel.DataAnnotations;

namespace AccountAndJwt.Contracts.Models
{
	/// <summary>
	/// User registration data model
	/// </summary>
	public class RegisterUserAm
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

		/// <summary>
		/// First name
		/// </summary>
		[Required]
		public String FirstName { get; set; }

		/// <summary>
		/// Last name
		/// </summary>
		[Required]
		public String LastName { get; set; }

		/// <summary>
		/// E-Mail
		/// </summary>
		[Required]
		public String Email { get; set; }
	}
}