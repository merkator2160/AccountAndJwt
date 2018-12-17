using System;
using System.ComponentModel.DataAnnotations;

namespace AccountAndJwt.Contracts.Models
{
    /// <summary>
    /// Change first and last user name request model
    /// </summary>
    public class ChangeNameRequestAm
    {
        /// <summary>
        /// First name
        /// </summary>
        [Required]
        public String FirstName { get; set; }

        /// <summary>
        /// Last name
        /// </summary>
        [Required]
        public String LastName { get; set; }
    }
}