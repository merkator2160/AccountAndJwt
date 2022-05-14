using Microsoft.AspNetCore.WebUtilities;
using System.Text;
using System.Text.Json;

namespace AccountAndJwt.Ui.Utilities
{
    /// <summary>
    /// https://github.com/simonpbond/blazor.client.jwt
    /// </summary>
    public class JwtTokenParser
    {
        public AppToken ParseToken(String encodedToken)
        {
            var appToken = new AppToken();
            try
            {
                var tokenSections = encodedToken.Split('.');

                var decodedHeaderBytes = WebEncoders.Base64UrlDecode(tokenSections[0]);
                var decodedHeaderJson = Encoding.UTF8.GetString(decodedHeaderBytes);
                var headerDictionary = new Dictionary<String, Object>();
                headerDictionary = JsonSerializer.Deserialize<Dictionary<String, Object>>(decodedHeaderJson);

                var decodedPayloadBytes = WebEncoders.Base64UrlDecode(tokenSections[1]);
                var decodedPayloadJson = Encoding.UTF8.GetString(decodedPayloadBytes);
                var payloadDictionary = new Dictionary<String, Object>();
                payloadDictionary = JsonSerializer.Deserialize<Dictionary<String, Object>>(decodedPayloadJson);


                appToken.Header = new AppToken.AppTokenHeader()
                {
                    TokenAlgorithm = headerDictionary.ContainsKey("alg") ? headerDictionary.SingleOrDefault(claim => claim.Key == "alg").Value.ToString() : String.Empty,
                    TokenType = headerDictionary.ContainsKey("typ") ? headerDictionary.SingleOrDefault(claim => claim.Key == "typ").Value.ToString() : String.Empty
                };

                var dateTimeEpoch = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);

                appToken.Payload = new AppToken.AppTokenPayload()
                {
                    Claims = payloadDictionary.ToDictionary(claims => claims.Key, claims => claims.Value.ToString()),

                    TokenJwtIdentifier = payloadDictionary.ContainsKey("jti") ? payloadDictionary.SingleOrDefault(claim => claim.Key == "jti").Value.ToString() : String.Empty,
                    TokenUniqueName = payloadDictionary.ContainsKey("unique_name") ? payloadDictionary.SingleOrDefault(claim => claim.Key == "unique_name").Value.ToString() : String.Empty,
                    TokenSubject = payloadDictionary.ContainsKey("sub") ? payloadDictionary.SingleOrDefault(claim => claim.Key == "sub").Value.ToString() : String.Empty,
                    TokenAudience = payloadDictionary.ContainsKey("aud") ? payloadDictionary.SingleOrDefault(claim => claim.Key == "aud").Value.ToString() : String.Empty,
                    TokenExpirationTime = dateTimeEpoch.AddSeconds(Int32.Parse(payloadDictionary.ContainsKey("exp") ? payloadDictionary.SingleOrDefault(claim => claim.Key == "exp").Value.ToString() : "0")),
                    TokenNotBeforeTime = dateTimeEpoch.AddSeconds(Int32.Parse(payloadDictionary.ContainsKey("nbf") ? payloadDictionary.SingleOrDefault(claim => claim.Key == "nbf").Value.ToString() : "0")),
                    TokenIssuedAt = dateTimeEpoch.AddSeconds(Int32.Parse(payloadDictionary.ContainsKey("iat") ? payloadDictionary.SingleOrDefault(claim => claim.Key == "iat").Value.ToString() : "0")),
                    TokenIssuer = payloadDictionary.ContainsKey("iss") ? payloadDictionary.SingleOrDefault(claim => claim.Key == "iss").Value.ToString() : String.Empty
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: Could not parse token: {ex.Message}\n{ex.StackTrace}");
            }

            return appToken;
        }

        public class AppToken
        {
            public AppTokenHeader Header { get; set; }
            public AppTokenPayload Payload { get; set; }

            public String ToJson()
            {
                var json = JsonSerializer.Serialize(this);
                return json;
            }

            public class AppTokenHeader
            {
                public String TokenAlgorithm { get; set; }
                public String TokenType { get; set; }
            }

            public class AppTokenPayload
            {
                public String TokenIssuer { get; set; }
                public String TokenSubject { get; set; }
                public String TokenUniqueName { get; set; }
                public String TokenAudience { get; set; }
                public DateTime TokenExpirationTime { get; set; }
                public DateTime TokenNotBeforeTime { get; set; }
                public DateTime TokenIssuedAt { get; set; }
                public String TokenJwtIdentifier { get; set; }
                public Dictionary<String, String> Claims { get; set; } = new Dictionary<String, String>();

                public String GetClaimValueByKey(String key)
                {
                    var value = String.Empty;
                    Claims.TryGetValue(key, out value);
                    return value;

                }

                public Boolean ClaimExists(String key)
                {
                    if (Claims.ContainsKey(key)) { return true; } else { return false; }
                }
            }
        }
    }
}