﻿namespace AccountAndJwt.Ui.Utilities.TokenParser.Models
{
    public class AppToken
    {
        public AppTokenHeader Header { get; set; }
        public AppTokenPayload Payload { get; set; }
    }
}