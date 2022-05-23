using System.ComponentModel.DataAnnotations;

namespace AccountAndJwt.Contracts.Models.Api.Request
{
    public class ChangeNameRequestAm
    {
        [Required]
        public String FirstName { get; set; }

        [Required]
        public String LastName { get; set; }
    }
}