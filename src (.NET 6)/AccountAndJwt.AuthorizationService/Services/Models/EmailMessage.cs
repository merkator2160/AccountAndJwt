namespace AccountAndJwt.AuthorizationService.Services.Models
{
    public class EmailMessage
    {
        public String Subject { get; set; }
        public String Body { get; set; }
        public String Destination { get; set; }
    }
}