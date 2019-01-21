﻿using AccountAndJwt.Common.Consts;
using AccountAndJwt.Common.Helpers;
using AccountAndJwt.Database.Models.Storage;
using System;
using System.Linq;

namespace AccountAndJwt.Database
{
	public static class DataSeeder
	{
		public static void AddInitialData(this DataContext context, String passwordSalt)
		{
			context.AddRoles();

			var roles = context.Roles.ToArray();
			context.AddUsers(roles, passwordSalt);
		}
		private static void AddRoles(this DataContext context)
		{
			if(!context.Roles.Any(p => p.Name.Equals(Role.Admin)))
			{
				context.Roles.Add(new RoleDb()
				{
					Name = Role.Admin
				});
			}
			if(!context.Roles.Any(p => p.Name.Equals(Role.Moderator)))
			{
				context.Roles.Add(new RoleDb()
				{
					Name = Role.Moderator
				});
			}

			context.SaveChanges();
		}
		private static void AddUsers(this DataContext context, RoleDb[] roles, String passwordSalt)
		{
			if(!context.Users.Any(p => p.Login.Equals("guest")))
			{
				context.Users.Add(new UserDb
				{
					Login = "guest",
					PasswordHash = KeyHelper.CreatePasswordHash("HtR00MtOxKyHUg7359QL", passwordSalt),
					Email = "2160@inbox.ru",
					FirstName = "Peter",
					LastName = "Wilson"
				});
			}
			if(!context.Users.Any(p => p.Login.Equals("admin")))
			{
				context.Users.Add(new UserDb
				{
					Login = "admin",
					PasswordHash = KeyHelper.CreatePasswordHash("ipANWvuFUA5e2qWk0iTd", passwordSalt),
					Email = "2160@inbox.ru",
					FirstName = "Mett",
					LastName = "Hardy",
					UserRoles = new[]
					{
						new UserRoleDb()
						{
							Role = roles[0]
						},
						new UserRoleDb()
						{
							Role = roles[1]
						}
					}
				});
			}

			context.SaveChanges();
		}
	}
}