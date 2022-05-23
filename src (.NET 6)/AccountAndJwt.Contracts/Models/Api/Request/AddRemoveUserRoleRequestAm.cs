using System.ComponentModel.DataAnnotations;

namespace AccountAndJwt.Contracts.Models.Api.Request
{
    public class AddRemoveUserRoleRequestAm
    {
        [Required]
        [Range(1, Int32.MaxValue, ErrorMessage = "Id value min: 1, max: Int32.MaxValue")]
        public Int32 UserId { get; set; }

        [Required]
        [Range(1, Int32.MaxValue, ErrorMessage = "Id value min: 1, max: Int32.MaxValue")]
        public Int32 RoleId { get; set; }
    }
}