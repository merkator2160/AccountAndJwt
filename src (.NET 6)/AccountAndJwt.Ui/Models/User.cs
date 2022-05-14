using AccountAndJwt.Contracts.Models.Api;

namespace AccountAndJwt.Ui.Models
{
    internal class User
    {
        public Int32 Id { get; set; }
        public String FirstName { get; set; }
        public String LastName { get; set; }
        public String Email { get; set; }

        public Boolean IsAuthorized { get; set; }
        public AuthorizeResponseAm Tokens { get; set; }
        public DateTime TokenExpirationTimeUtc { get; set; }
    }
}