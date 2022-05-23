using System.ComponentModel.DataAnnotations;

namespace AccountAndJwt.Contracts.Models.Api.Request
{
    public class AuthorizeRequestAm
    {
        [Required]
        public String Login { get; set; }

        [Required]
        public String Password { get; set; }
    }
}