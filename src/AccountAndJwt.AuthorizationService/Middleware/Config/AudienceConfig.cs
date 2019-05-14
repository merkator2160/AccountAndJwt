using System;

namespace AccountAndJwt.AuthorizationService.Middleware.Config
{
	public class AudienceConfig
	{
		public String Secret { get; set; }
		public String ValidIssuer { get; set; }
		public String ValidAudience { get; set; }
		public Int32 TokenLifetimeSec { get; set; }
		public String PasswordSalt { get; set; }
	}
}
