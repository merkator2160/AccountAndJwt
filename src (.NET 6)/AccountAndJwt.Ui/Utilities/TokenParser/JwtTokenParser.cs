using AccountAndJwt.Ui.Utilities.TokenParser.Models;
using Microsoft.AspNetCore.WebUtilities;
using System.Security.Claims;
using System.Text;
using System.Text.Json;

namespace AccountAndJwt.Ui.Utilities.TokenParser
{
    public static class JwtTokenParser
    {
        public static AppToken ParseToken(this String encodedToken)
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


                appToken.Header = new AppTokenHeader()
                {
                    TokenAlgorithm = headerDictionary.ContainsKey("alg") ? headerDictionary.SingleOrDefault(claim => claim.Key == "alg").Value.ToString() : String.Empty,
                    TokenType = headerDictionary.ContainsKey("typ") ? headerDictionary.SingleOrDefault(claim => claim.Key == "typ").Value.ToString() : String.Empty
                };

                var dateTimeEpoch = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
                appToken.Payload = new AppTokenPayload()
                {
                    Sid = payloadDictionary.ContainsKey(ClaimTypes.Sid) ? Int32.Parse(payloadDictionary.SingleOrDefault(claim => claim.Key == ClaimTypes.Sid).Value.ToString()) : 0,
                    FirstName = payloadDictionary.ContainsKey(ClaimTypes.Name) ? payloadDictionary.SingleOrDefault(claim => claim.Key == ClaimTypes.Name).Value.ToString() : String.Empty,
                    LastName = payloadDictionary.ContainsKey(ClaimTypes.Surname) ? payloadDictionary.SingleOrDefault(claim => claim.Key == ClaimTypes.Surname).Value.ToString() : String.Empty,
                    Email = payloadDictionary.ContainsKey(ClaimTypes.Email) ? payloadDictionary.SingleOrDefault(claim => claim.Key == ClaimTypes.Email).Value.ToString() : String.Empty,

                    TokenAudience = payloadDictionary.ContainsKey("aud") ? payloadDictionary.SingleOrDefault(claim => claim.Key == "aud").Value.ToString() : String.Empty,
                    TokenExpirationTime = dateTimeEpoch.AddSeconds(Int32.Parse(payloadDictionary.ContainsKey("exp") ? payloadDictionary.SingleOrDefault(claim => claim.Key == "exp").Value.ToString() : "0")),
                    TokenNotBeforeTime = dateTimeEpoch.AddSeconds(Int32.Parse(payloadDictionary.ContainsKey("nbf") ? payloadDictionary.SingleOrDefault(claim => claim.Key == "nbf").Value.ToString() : "0")),
                    TokenIssuedAt = dateTimeEpoch.AddSeconds(Int32.Parse(payloadDictionary.ContainsKey("iat") ? payloadDictionary.SingleOrDefault(claim => claim.Key == "iat").Value.ToString() : "0")),
                    TokenIssuer = payloadDictionary.ContainsKey("iss") ? payloadDictionary.SingleOrDefault(claim => claim.Key == "iss").Value.ToString() : String.Empty,

                    ClaimDictionary = payloadDictionary
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: Could not parse token: {ex.Message}\n{ex.StackTrace}");
            }

            return appToken;
        }
        public static Claim[] ToClaims(this Dictionary<String, Object> payloadDictionary)
        {
            var claimList = new List<Claim>();

            foreach (var x in payloadDictionary)
            {
                if (!x.Key.Equals(ClaimTypes.Role))
                {
                    claimList.Add(new Claim(x.Key, x.Value.ToString()));
                    continue;
                }

                var roles = JsonSerializer.Deserialize<String[]>(x.Value.ToString());
                claimList.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));
            }

            return claimList.ToArray();
        }
    }
}