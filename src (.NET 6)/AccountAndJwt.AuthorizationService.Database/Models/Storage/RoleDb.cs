namespace AccountAndJwt.AuthorizationService.Database.Models.Storage
{
    public class RoleDb
    {
        public Int32 Id { get; set; }
        public String Name { get; set; }

        public ICollection<UserRoleDb> UserRoles { get; set; }
    }
}