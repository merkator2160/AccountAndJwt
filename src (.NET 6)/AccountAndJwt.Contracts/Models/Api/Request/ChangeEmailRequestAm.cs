using System.ComponentModel.DataAnnotations;

namespace AccountAndJwt.Contracts.Models.Api.Request
{
    public class ChangeEmailRequestAm
    {
        [Required]
        [EmailAddress(ErrorMessage = "Incorrect E-Mail address!")]
        public String NewEmail { get; set; }
    }
}