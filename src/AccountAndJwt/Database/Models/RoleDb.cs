﻿using System;
using System.Collections.Generic;

namespace AccountAndJwt.Api.Database.Models
{
	public class RoleDb
	{
		public Int32 Id { get; set; }
		public String RoleName { get; set; }

		public ICollection<UserRoleDb> UserRoles { get; set; }
	}
}