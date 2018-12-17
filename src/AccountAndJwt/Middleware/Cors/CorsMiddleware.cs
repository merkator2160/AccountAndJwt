using Microsoft.AspNetCore.Cors.Infrastructure;

namespace AccountAndJwt.Api.Middleware.Cors
{
	internal static class CorsMiddleware
	{
		public static void AddPolitics(CorsOptions options)
		{
			options.AddPolicy(CorsPolicies.Development, AddDevelopmentPolicy);
			options.AddPolicy(CorsPolicies.Production, AddProductionPolicy);
			options.AddPolicy(CorsPolicies.Staging, AddStagingPolicy);
		}
		private static void AddDevelopmentPolicy(CorsPolicyBuilder builder)
		{
			builder
				.AllowAnyOrigin()
				.AllowAnyMethod()
				.AllowAnyHeader()
				.AllowCredentials();
		}
		private static void AddProductionPolicy(CorsPolicyBuilder builder)
		{
			builder
				// Temp
				//.WithOrigins("http://localhost:4200")   //TODO: we should determine and add a list of allowed hosts for production server
				.AllowAnyOrigin()
				.AllowAnyMethod()
				.AllowAnyHeader()
				.AllowCredentials();
		}
		private static void AddStagingPolicy(CorsPolicyBuilder builder)
		{
			builder
				.AllowAnyOrigin()
				.AllowAnyMethod()
				.AllowAnyHeader()
				.AllowCredentials();
		}
	}
}