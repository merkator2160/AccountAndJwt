using AccountAndJwt.Common.Const;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using System;
using System.Globalization;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace AccountAndJwt.AuthorizationService.Middleware
{
	internal static class ErrorHandlingMiddleware
	{
		/// <summary>
		/// It don't allow information from unhandled exceptions leave application borders. 
		/// Also provides these exceptions logging.
		/// </summary>
		public static void UseGlobalExceptionHandler(this IApplicationBuilder app)
		{
			app.UseExceptionHandler(errorApp =>
			{
				errorApp.Run(async context =>
				{
					var exceptionHandlerFeature = context.Features.Get<IExceptionHandlerFeature>();

					var ex = exceptionHandlerFeature.Error;
					if(ex is ApplicationException)
					{
						context.Response.StatusCode = 460;
						context.Response.ContentType = HttpMimeType.Text.Plain;

						await context.Response.WriteAsync(ex.Message, Encoding.UTF8);

						return;
					}

					var logger = app.ApplicationServices.GetService<ILogger<Startup>>();
					logger.LogError(ex, ex.Message);

					context.Response.StatusCode = 500;
					context.Response.ContentType = HttpMimeType.Text.Plain;
#if DEBUG
					await context.Response.WriteAsync(SerializeByNewtonsoftJson(ex), Encoding.UTF8);
#else
					await context.Response.WriteAsync("Internal server error!", Encoding.UTF8);
#endif
				});
			});
		}
		private static String SerializeByNewtonsoftJson(Exception ex)
		{
			return JsonConvert.SerializeObject(ex, new JsonSerializerSettings()
			{
				ContractResolver = new CamelCasePropertyNamesContractResolver(),
				Converters =
				{
					new StringEnumConverter(),
					new IsoDateTimeConverter
					{
						DateTimeStyles = DateTimeStyles.AssumeUniversal
					}
				}
			});
		}
		private static String SerializeByMicrosoftJson(Exception ex)
		{
			return System.Text.Json.JsonSerializer.Serialize(ex, new JsonSerializerOptions()
			{
				PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
				Converters = { new JsonStringEnumConverter(JsonNamingPolicy.CamelCase) }
			});
		}
	}
}