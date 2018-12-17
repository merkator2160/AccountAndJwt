using System;

namespace AccountAndJwt.Services.Models
{
    public class CreateAccessTokenByCredentialsDto
    {
        public String AccessToken { get; set; }
        public Double AccessTokenLifeTime { get; set; }
        public String RefreshToken { get; set; }
    }
}