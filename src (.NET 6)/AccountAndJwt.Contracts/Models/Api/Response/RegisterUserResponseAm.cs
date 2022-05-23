namespace AccountAndJwt.Contracts.Models.Api.Response
{
    public class RegisterUserResponseAm
    {
        public String AccessTokenUrl { get; set; }
        public String RefreshAccessTokenUrl { get; set; }
        public String RevokeRefreshTokenUrl { get; set; }
    }
}