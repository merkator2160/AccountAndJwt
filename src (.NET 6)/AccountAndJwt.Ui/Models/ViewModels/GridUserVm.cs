using AccountAndJwt.Contracts.Models.Api;

namespace AccountAndJwt.Ui.Models.ViewModels
{
    public class GridUserVm
    {
        public Int32 Id { get; set; }
        public String Login { get; set; }
        public String FirstName { get; set; }
        public String LastName { get; set; }
        public String Email { get; set; }
        public RoleAm[] Roles { get; set; }
    }
}