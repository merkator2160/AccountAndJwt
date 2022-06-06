using AccountAndJwt.Contracts.Const;
using System.ComponentModel.DataAnnotations;

namespace AccountAndJwt.Contracts.Models.Api.Request
{
    public class RegisterUserRequestAm
    {
        [Required]
        [MaxLength(Limits.User.LoginMaxLength)]
        public String Login { get; set; }

        // TODO: Password validation policy attribute required.
        [Required]
        public String Password { get; set; }

        [Required]
        [Compare(nameof(Password), ErrorMessage = "Password and Confirm password must match")]
        public String ConfirmPassword { get; set; }

        [Required]
        [MaxLength(Limits.User.FirstNameMaxLength)]
        public String FirstName { get; set; }

        [Required]
        [MaxLength(Limits.User.LastNameMaxLength)]
        public String LastName { get; set; }

        [Required]
        [EmailAddress(ErrorMessage = "Incorrect E-Mail address!")]
        public String Email { get; set; }
    }
}