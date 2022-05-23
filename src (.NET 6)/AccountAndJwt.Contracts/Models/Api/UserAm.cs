namespace AccountAndJwt.Contracts.Models.Api
{
    public class UserAm
    {
        public Int32 Id { get; set; }
        public String Login { get; set; }
        public String FirstName { get; set; }
        public String LastName { get; set; }
        public String Email { get; set; }
        public RoleAm[] Roles { get; set; }
    }
}