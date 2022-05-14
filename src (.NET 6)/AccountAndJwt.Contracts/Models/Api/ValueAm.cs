using System.ComponentModel.DataAnnotations;

namespace AccountAndJwt.Contracts.Models.Api
{
    /// <summary>
    /// Value model
    /// </summary>
    public class ValueAm
    {
        /// <summary>
        /// Value unique identifier
        /// </summary>
        [Required]
        public Int32 Id { get; set; }

        /// <summary>
        /// Value data
        /// </summary>
        [Required]
        public Int32 Value { get; set; }

        /// <summary>
        /// Value commentary
        /// </summary>
        [Required]
        public String Commentary { get; set; }
    }
}