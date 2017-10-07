using System;

namespace AccountAndJwt.Client
{
    internal class Token
    {
        public String AccessToken { get; set; }
        public Double AccessTokenLifeTime { get; set; }
        public String RefreshToken { get; set; }
    }
}