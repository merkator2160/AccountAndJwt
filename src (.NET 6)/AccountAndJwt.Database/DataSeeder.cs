using AccountAndJwt.Common.Consts;
using AccountAndJwt.Common.Helpers;
using AccountAndJwt.Database.Models.Storage;
using System;
using System.Linq;

namespace AccountAndJwt.Database
{
	public static class DataSeeder
	{
		public static void PopulateDatabase(this DataContext context, String passwordSalt)
		{
			context.AddRoles();
			context.AddUsers(passwordSalt);
			context.AddValues();
		}
		public static void AddRoles(this DataContext context)
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
		public static void AddUsers(this DataContext context, String passwordSalt)
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
							RoleId = context.Roles.First(p => p.Name.Equals(Role.Admin)).Id
						},
						new UserRoleDb()
						{
							RoleId = context.Roles.First(p => p.Name.Equals(Role.Moderator)).Id
						}
					}
				});
			}

			context.SaveChanges();
		}
		public static void AddValues(this DataContext context)
		{
			if(context.Values.Any())
				return;

			for(var i = 100; i < 200; i++)
			{
				context.Values.Add(new ValueDb()
				{
					Value = i,
					Commentary = $"Value: {i}"
				});
			}

			context.SaveChanges();
		}
	}
}