namespace AccountAndJwt.AuthorizationService.Services.Models
{
    public class CreateAccessTokenByCredentialsDto
    {
        public String AccessToken { get; set; }
        public String RefreshToken { get; set; }
    }
}