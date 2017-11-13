using System;

namespace AccountAndJwt.Services.Models
{
    public class EmailMessage
    {
        public String Subject { get; set; }
        public String Body { get; set; }
        public String Destination { get; set; }
    }
}