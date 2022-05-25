using System.ComponentModel.DataAnnotations;

namespace AccountAndJwt.Contracts.Models.Api.Request
{
    public class RegisterUserRequestAm
    {
        [Required]
        public String Login { get; set; }

        [Required]
        public String Password { get; set; }

        [Required]
        [Compare(nameof(Password), ErrorMessage = "Password and Confirm password must match")]
        public String ConfirmPassword { get; set; }

        [Required]
        public String FirstName { get; set; }

        [Required]
        public String LastName { get; set; }

        [Required]
        [EmailAddress(ErrorMessage = "Incorrect E-Mail address!")]
        public String Email { get; set; }
    }
}