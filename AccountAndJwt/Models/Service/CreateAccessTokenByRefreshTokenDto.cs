using System;

namespace AccountAndJwt.Models.Service
{
    public class CreateAccessTokenByRefreshTokenDto
    {
        public String AccessToken { get; set; }
        public Double AccessTokenLifeTime { get; set; }
    }
}