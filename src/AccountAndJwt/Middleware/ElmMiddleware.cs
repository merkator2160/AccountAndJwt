using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;

namespace AccountAndJwt.Api.Middleware
{
	internal static class ElmMiddleware
	{
		public static void AddConfiguredElm(this IServiceCollection services, IConfiguration configuration)
		{
			services.AddElm(options =>
			{
				options.Path = new PathString(configuration["ElmLogging:Url"]);
				options.Filter = (name, level) => level >= (LogLevel)Enum.Parse(typeof(LogLevel), configuration["ElmLogging:LogLevel"]);
			});
		}
	}
}