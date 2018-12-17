using System;

namespace AccountAndJwt.Api.Middleware.Cors
{
	internal static class CorsPolicies
	{
		public const String Development = "DevelopmentPolicy";
		public const String Production = "ProductionPolicy";
		public const String Staging = "StagingPolicy";
	}
}