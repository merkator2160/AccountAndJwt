using AccountAndJwt.Contracts.Const;
using System.ComponentModel.DataAnnotations;

namespace AccountAndJwt.Contracts.Models.Api.Request
{
    public class RegisterUserRequestAm
    {
        [Required]
        [MaxLength(Limits.User.LoginMaxLength)]
        public String Login { get; set; }

        // TODO: Better password validation policy attribute required.
        [Required]
        [DataType(DataType.Password)]
        [StringLength(Limits.Password.MaxLength, MinimumLength = Limits.Password.MinLength)]
        public String Password { get; set; }

        // TODO: Better password validation policy attribute required.
        [Required]
        [DataType(DataType.Password)]
        [StringLength(Limits.Password.MaxLength, MinimumLength = Limits.Password.MinLength)]
        [Compare(nameof(Password), ErrorMessage = "Fields \"Password\" and \"ConfirmPassword\" must match!")]
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