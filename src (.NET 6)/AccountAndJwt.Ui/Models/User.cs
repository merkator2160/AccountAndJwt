using AccountAndJwt.Contracts.Models.Api.Response;
using AccountAndJwt.Ui.Utilities.TokenParser.Models;

namespace AccountAndJwt.Ui.Models
{
    public class User
    {
        public Int32 Id { get; set; }
        public String FirstName { get; set; }
        public String LastName { get; set; }
        public String Email { get; set; }

        public Boolean IsAuthorized { get; set; }

        public AuthorizeResponseAm ServerTokens { get; set; }
        public AppToken ParsedToken { get; set; }

        public DateTime TokenExpirationTimeUtc { get; set; }
    }
}