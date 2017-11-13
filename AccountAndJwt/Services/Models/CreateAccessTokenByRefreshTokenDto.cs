using System;

namespace AccountAndJwt.Services.Models
{
    public class CreateAccessTokenByRefreshTokenDto
    {
        public String AccessToken { get; set; }
        public Double AccessTokenLifeTime { get; set; }
    }
}