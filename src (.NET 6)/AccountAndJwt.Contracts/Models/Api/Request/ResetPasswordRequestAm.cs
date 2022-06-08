using AccountAndJwt.Contracts.Const;
using System.ComponentModel.DataAnnotations;

namespace AccountAndJwt.Contracts.Models.Api.Request
{
    public class ResetPasswordRequestAm
    {
        // TODO: Better password validation policy attribute required.
        [Required]
        [DataType(DataType.Password)]
        [StringLength(Limits.Password.MaxLength, MinimumLength = Limits.Password.MinLength)]
        public String OldPassword { get; set; }

        // TODO: Better password validation policy attribute required.
        [Required]
        [DataType(DataType.Password)]
        [StringLength(Limits.Password.MaxLength, MinimumLength = Limits.Password.MinLength)]
        public String NewPassword { get; set; }

        // TODO: Better password validation policy attribute required.
        [Required]
        [DataType(DataType.Password)]
        [StringLength(Limits.Password.MaxLength, MinimumLength = Limits.Password.MinLength)]
        [Compare(nameof(NewPassword), ErrorMessage = "Fields \"NewPassword\" and \"ConfirmPassword\" must match!")]
        public String ConfirmPassword { get; set; }
    }
}