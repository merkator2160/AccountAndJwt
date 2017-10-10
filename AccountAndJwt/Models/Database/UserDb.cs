using System;
using System.Collections.Generic;

namespace AccountAndJwt.Models.Database
{
    public class UserDb
    {
        public Int32 Id { get; set; }
        public String Login { get; set; }
        public String PasswordHash { get; set; }
        public String FirstName { get; set; }
        public String LastName { get; set; }
        public String Email { get; set; }
        public String RefreshToken { get; set; }

        public ICollection<UserRoleDb> UserRoles { get; set; }
    }
}