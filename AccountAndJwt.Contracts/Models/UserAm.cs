using System;

namespace AccountAndJwt.Contracts.Models
{
    /// <summary>
    /// User information response
    /// </summary>
    public class UserAm
    {
        /// <summary>
        /// Unique user identifier
        /// </summary>
        public Int32 Id { get; set; }

        /// <summary>
        /// User login
        /// </summary>
        public String Login { get; set; }

        /// <summary>
        /// User password
        /// </summary>
        public String PasswordHash { get; set; }

        /// <summary>
        /// First user name
        /// </summary>
        public String FirstName { get; set; }

        /// <summary>
        /// Last user name
        /// </summary>
        public String LastName { get; set; }

        /// <summary>
        /// E-Mail
        /// </summary>
        public String Email { get; set; }

        /// <summary>
        /// Roles assigned to the user
        /// </summary>
        public String[] Roles { get; set; }
    }
}