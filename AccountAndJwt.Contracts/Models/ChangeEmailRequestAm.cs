using System;
using System.ComponentModel.DataAnnotations;

namespace AccountAndJwt.Contracts.Models
{
    /// <summary>
    /// Changing Email request
    /// </summary>
    public class ChangeEmailRequestAm
    {
        /// <summary>
        /// Unique client id
        /// </summary>
        [Required]
        public String ClientId { get; set; }

        /// <summary>
        /// New Email
        /// </summary>
        [Required]
        public String NewEmail { get; set; }
    }
}