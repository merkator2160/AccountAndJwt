using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace AccountAndJwt.Api.Middleware
{
	internal static class JwtAuthMiddleware
	{
		public static void AddJwtAuthService(this IServiceCollection services, IConfiguration configurationService)
		{
			services.AddAuthentication(options =>
			{
				options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
				options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
			})
			.AddJwtBearer(o =>
			{
				o.RequireHttpsMetadata = false;     // switch off SSL, HTTP instead HTTPS
				o.TokenValidationParameters = new TokenValidationParameters()
				{
					ValidateIssuer = true,
					ValidIssuer = configurationService["AudienceConfig:ValidIssuer"],

					ValidateAudience = true,
					ValidAudience = configurationService["AudienceConfig:ValidAudience"],

					ValidateIssuerSigningKey = true,
					IssuerSigningKey = CreateSigningKey(configurationService["AudienceConfig:Secret"]),

					ValidateLifetime = true
				};
				o.Events = new JwtBearerEvents()
				{
					OnAuthenticationFailed = OnAuthenticationFailed,
					OnChallenge = OnChallenge,
					OnMessageReceived = OnMessageReceived,
					OnTokenValidated = OnTokenValidated
				};
			});
		}
		public static SymmetricSecurityKey CreateSigningKey(String x64Secret)
		{
			return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(x64Secret));
		}
		public static Int32 GetId(this ClaimsPrincipal user)
		{
			return Int32.Parse(user.Claims.ToArray().First(p => p.Type.Contains("nameidentifier")).Value);
		}


		// HANDLERS ///////////////////////////////////////////////////////////////////////////////
		private static Task OnAuthenticationFailed(AuthenticationFailedContext authenticationFailedContext)
		{
			// TODO: Maybe add logging in future
			return Task.CompletedTask;
		}
		private static Task OnChallenge(JwtBearerChallengeContext jwtBearerChallengeContext)
		{
			// TODO: Maybe add logging in future
			return Task.CompletedTask;
		}
		private static Task OnMessageReceived(MessageReceivedContext messageReceivedContext)
		{
			// TODO: Maybe add logging in future
			return Task.CompletedTask;
		}
		private static Task OnTokenValidated(TokenValidatedContext tokenValidatedContext)
		{
			// TODO: Maybe add logging in future
			return Task.CompletedTask;
		}
	}
}