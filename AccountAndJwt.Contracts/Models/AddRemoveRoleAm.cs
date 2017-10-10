using System;
using System.ComponentModel.DataAnnotations;

namespace AccountAndJwt.Contracts.Models
{
    /// <summary>
    /// Model used in changing user roles requests
    /// </summary>
    public class AddRemoveRoleAm
    {
        /// <summary>
        /// Client unique identifier
        /// </summary>
        [Required]
        public Int32 UserId { get; set; }

        /// <summary>
        /// Role for adding or removing
        /// </summary>
        [Required]
        public String Role { get; set; }
    }
}