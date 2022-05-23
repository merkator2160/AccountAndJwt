using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;

namespace AccountAndJwt.AuthorizationService.Middleware
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
#if DEBUG
                o.Events = new JwtBearerEvents()
                {
                    OnAuthenticationFailed = OnAuthenticationFailed,
                    OnChallenge = OnChallenge,
                    OnMessageReceived = OnMessageReceived,
                    OnTokenValidated = OnTokenValidated
                };
#endif
            });
        }
        public static SymmetricSecurityKey CreateSigningKey(String x64Secret)
        {
            return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(x64Secret));
        }
        public static Int32 GetId(this ClaimsPrincipal user)
        {
            return Int32.Parse(user.Claims.ToArray().First(p => p.Type.Contains(ClaimTypes.Sid)).Value);
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