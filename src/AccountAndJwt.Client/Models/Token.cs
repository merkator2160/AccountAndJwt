using System;

namespace AccountAndJwt.Client.Models
{
    internal class Token
    {
        public String AccessToken { get; set; }
        public Double AccessTokenLifeTime { get; set; }
        public String RefreshToken { get; set; }
    }
}