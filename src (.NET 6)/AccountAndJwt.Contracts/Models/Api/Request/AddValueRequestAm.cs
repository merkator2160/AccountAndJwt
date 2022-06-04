using AccountAndJwt.Contracts.Const;
using System.ComponentModel.DataAnnotations;

namespace AccountAndJwt.Contracts.Models.Api.Request
{
    public class AddValueRequestAm
    {
        [Required]
        public Int32? Value { get; set; }

        [MaxLength(Limits.Value.CommentaryMaxLength)]
        public String Commentary { get; set; }
    }
}