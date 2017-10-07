using System;

namespace AccountAndJwt.Models.Database
{
    public class RefreshTokenDb
    {
        public Int32 Id { get; set; }
        public Guid ClientId { get; set; }
        public String Value { get; set; }
    }
}