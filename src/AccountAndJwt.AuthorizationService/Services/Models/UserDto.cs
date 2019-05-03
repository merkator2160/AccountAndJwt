﻿using System;

namespace AccountAndJwt.AuthorizationService.Services.Models
{
	public class UserDto
	{
		public Int32 Id { get; set; }
		public String Login { get; set; }
		public String PasswordHash { get; set; }
		public String FirstName { get; set; }
		public String LastName { get; set; }
		public String Email { get; set; }
		public String[] Roles { get; set; }
	}
}