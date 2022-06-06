using AccountAndJwt.Contracts.Const;
using AccountAndJwt.Contracts.Models.Api;
using System.ComponentModel.DataAnnotations;

namespace AccountAndJwt.Ui.Models.ViewModels
{
    public class GridUserVm
    {
        public Int32 Id { get; set; }

        [Required]
        [MaxLength(Limits.User.LoginMaxLength)]
        public String Login { get; set; }

        // TODO: Password validation policy attribute required.
        [Required]
        public String Password { get; set; }

        [Required]
        [MaxLength(Limits.User.FirstNameMaxLength)]
        public String FirstName { get; set; }

        [Required]
        [MaxLength(Limits.User.LastNameMaxLength)]
        public String LastName { get; set; }

        [EmailAddress(ErrorMessage = "Incorrect E-Mail address!")]
        public String Email { get; set; }

        public List<RoleAm> RoleList { get; set; }
    }
}