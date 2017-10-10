using AccountAndJwt.Middleware;
using AccountAndJwt.Models.Database;
using System;

namespace AccountAndJwt.Database
{
    internal static class DataSeeder
    {
        public static void AddInitialData(this DataContext context, String passwordSalt)
        {
            var roles = context.AddRoles();

            context.AddUsers(roles, passwordSalt);
            context.AddValues();
            context.SaveChanges();
        }
        private static RoleDb[] AddRoles(this DataContext context)
        {
            var roles = new[]
            {
                new RoleDb()
                {
                    RoleName = "Admin"
                },
                new RoleDb()
                {
                    RoleName = "Moderator"
                }
            };
            context.Roles.AddRange(roles);
            return roles;
        }
        private static void AddUsers(this DataContext context, RoleDb[] roles, String passwordSalt)
        {
            context.Users.AddRange(new UserDb
            {
                Login = "Member",
                PasswordHash = JwtAuthMiddleware.CreatePasswordHash("123", passwordSalt),
                Email = "2160@inbox.ru",
                FirstName = "Peter",
                LastName = "Wilson"
            }, new UserDb
            {
                Login = "SuperUser",
                PasswordHash = JwtAuthMiddleware.CreatePasswordHash("1235", passwordSalt),
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
        private static void AddValues(this DataContext context)
        {
            context.Values.AddRange(new[]
            {
                new ValueDb()
                {
                    Value = "value1"
                },
                new ValueDb()
                {
                    Value = "value2"
                }
            });
        }
    }
}