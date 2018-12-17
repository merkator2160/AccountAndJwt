using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Text;
using System.Threading.Tasks;

namespace AccountAndJwt.Middleware
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
        public static String CreatePasswordHash(String password, String salt)
        {
            return Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: Encoding.UTF8.GetBytes(salt),
                prf: KeyDerivationPrf.HMACSHA1,
                iterationCount: 10000,
                numBytesRequested: 256 / 8));
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