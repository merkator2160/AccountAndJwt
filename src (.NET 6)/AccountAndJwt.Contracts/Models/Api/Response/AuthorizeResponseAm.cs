namespace AccountAndJwt.Contracts.Models.Api.Response
{
    public class AuthorizeResponseAm
    {
        public String AccessToken { get; set; }
        public String RefreshToken { get; set; }
    }
}