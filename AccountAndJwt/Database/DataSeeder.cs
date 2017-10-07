using AccountAndJwt.Models.Database;
using System;

namespace AccountAndJwt.Database
{
    internal static class DataSeeder
    {
        public static void AddInitialData(this DataContext context)
        {
            context.Users.AddRange(new UserDb
            {
                Id = Guid.NewGuid(),
                Login = "Member1",
                Password = "123",
                Email = "2160@inbox.ru",
                FirstName = "Peter",
                LastName = "Wilson"
            }, new UserDb
            {
                Id = Guid.NewGuid(),
                Login = "Member2",
                Password = "1235",
                Email = "2160@inbox.ru",
                FirstName = "Mett",
                LastName = "Hardy"
            });
        }

    }
}