using AccountAndJwt.Models.Enums;
using System;

namespace AccountAndJwt.Models.Database
{
    public class UserRolesDb
    {
        public Int32 Id { get; set; }
        public Role Role { get; set; }

        public UserDb User { get; set; }
    }
}