using System;

namespace AccountAndJwt.Contracts.Models
{
    /// <summary>
    /// User registration response
    /// </summary>
    public class RegisterUserResponseAm
    {
        /// <summary>
        /// Url of getting  access token and refresh token by provided login and password
        /// </summary>
        public String GetAccessTokenUrl { get; set; }

        /// <summary>
        /// Url of getting  access token by provided refresh token
        /// </summary>
        public String RefreshAccessTokenUrl { get; set; }
    }
}