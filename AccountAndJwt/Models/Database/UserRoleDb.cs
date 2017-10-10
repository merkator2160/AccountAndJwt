using System;

namespace AccountAndJwt.Models.Database
{
    public class UserRoleDb
    {
        public Int32 RoleId { get; set; }
        public RoleDb Role { get; set; }

        public Int32 UserId { get; set; }
        public UserDb User { get; set; }
    }
}