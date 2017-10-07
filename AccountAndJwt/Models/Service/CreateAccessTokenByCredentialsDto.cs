using System;

namespace AccountAndJwt.Models.Service
{
    public class CreateAccessTokenByCredentialsDto
    {
        public String AccessToken { get; set; }
        public Double AccessTokenLifeTime { get; set; }
        public String RefreshToken { get; set; }
    }
}