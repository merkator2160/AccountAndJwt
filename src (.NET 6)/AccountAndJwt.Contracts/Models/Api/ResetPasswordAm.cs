using System.ComponentModel.DataAnnotations;

namespace AccountAndJwt.Contracts.Models.Api
{
    /// <summary>
    /// Reset password model
    /// </summary>
    public class ResetPasswordAm
    {
        /// <summary>
        /// Old password
        /// </summary>
        [Required]
        public String OldPassword { get; set; }

        /// <summary>
        /// New password
        /// </summary>
        [Required]
        public String NewPassword { get; set; }
    }
}