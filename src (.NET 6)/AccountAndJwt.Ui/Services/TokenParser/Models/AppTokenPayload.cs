namespace AccountAndJwt.Ui.Utilities.TokenParser.Models
{
    public class AppTokenPayload
    {
        public Int32 Sid { get; set; }
        public String FirstName { get; set; }
        public String LastName { get; set; }
        public String Email { get; set; }

        public String TokenIssuer { get; set; }
        public String TokenAudience { get; set; }
        public DateTime TokenExpirationTime { get; set; }
        public DateTime TokenNotBeforeTime { get; set; }
        public DateTime TokenIssuedAt { get; set; }

        public Dictionary<String, Object> ClaimDictionary { get; set; }
    }
}