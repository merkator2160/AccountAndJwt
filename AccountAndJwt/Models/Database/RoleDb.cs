using System;
using System.Collections.Generic;

namespace AccountAndJwt.Models.Database
{
    public class RoleDb
    {
        public Int32 Id { get; set; }
        public String RoleName { get; set; }

        public ICollection<UserRoleDb> UserRoles { get; set; }
    }
}