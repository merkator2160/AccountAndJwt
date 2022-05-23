using System.ComponentModel.DataAnnotations;

namespace AccountAndJwt.Contracts.Models.Api.Request
{
    public class ResetPasswordRequestAm
    {
        [Required]
        public String OldPassword { get; set; }

        [Required]
        public String NewPassword { get; set; }
    }
}