using System;

namespace AccountAndJwt.Models.Service
{
    public class UserDto
    {
        public Guid Id { get; set; }
        public String Login { get; set; }
        public String Password { get; set; }
        public String FirstName { get; set; }
        public String LastName { get; set; }
        public String Email { get; set; }
        public String[] Roles { get; set; }
    }
}