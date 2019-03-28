using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Text;

namespace AccountAndJwt.Api.Middleware
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
					if(exceptionHandlerFeature == null)
						return;

					var ex = exceptionHandlerFeature.Error;
					if(ex is ApplicationException)
					{
						context.Response.StatusCode = 400;
						context.Response.ContentType = "text/plain";
						await context.Response.WriteAsync(ex.Message, Encoding.UTF8);

						return;
					}

					var logger = app.ApplicationServices.GetService<ILogger<Startup>>();
					logger.LogError(ex.Message);

					context.Response.StatusCode = 500;
					context.Response.ContentType = "text/plain";
#if DEBUG
					await context.Response.WriteAsync(ex.Message, Encoding.UTF8);
#else
					await context.Response.WriteAsync("Internal server error!", Encoding.UTF8);
#endif
				});
			});
		}
	}
}
