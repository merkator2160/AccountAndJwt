using System.ComponentModel.DataAnnotations;

namespace AccountAndJwt.Contracts.Models.Api.Request
{
    public class UpdateValueRequestAm
    {
        [Required]
        public Int32 Id { get; set; }

        [Required]
        public Int32 Value { get; set; }

        public String Commentary { get; set; }
    }
}