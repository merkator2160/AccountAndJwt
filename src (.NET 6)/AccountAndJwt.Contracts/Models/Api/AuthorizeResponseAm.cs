namespace AccountAndJwt.Contracts.Models.Api
{
    /// <summary>
    /// Result of authentication
    /// </summary>
    public class AuthorizeResponseAm
    {
        /// <summary>
        /// JWT access token itself
        /// </summary>
        public String AccessToken { get; set; }

        /// <summary>
        /// Access token lifetime
        /// </summary>
        public Int32 AccessTokenLifeTimeSec { get; set; }

        /// <summary>
        /// Refresh token
        /// </summary>
        public String RefreshToken { get; set; }
    }
}