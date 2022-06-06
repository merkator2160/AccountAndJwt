using AccountAndJwt.Contracts.Const;
using System.ComponentModel.DataAnnotations;

namespace AccountAndJwt.Contracts.Models.Api.Request
{
    public class ChangeNameRequestAm
    {
        [Required]
        [MaxLength(Limits.User.FirstNameMaxLength)]
        public String FirstName { get; set; }

        [Required]
        [MaxLength(Limits.User.LastNameMaxLength)]
        public String LastName { get; set; }
    }
}