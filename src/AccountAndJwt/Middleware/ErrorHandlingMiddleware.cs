﻿using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Text;

namespace AccountAndJwt.Api.Middleware
{
	internal static class ErrorHandlingMiddleware
	{
		public static void UseGlobalExceptionHandler(this IApplicationBuilder app)
		{
			app.UseExceptionHandler(errorApp =>
			{
				errorApp.Run(async context =>
				{
					var error = context.Features.Get<IExceptionHandlerFeature>();
					if(error != null)
					{
						var logger = app.ApplicationServices.GetService<ILogger<Startup>>();
						var ex = error.Error;

						logger.LogError(ex.Message);

						context.Response.StatusCode = 500;
						context.Response.ContentType = "text/plain";
						await context.Response.WriteAsync(ex.Message, Encoding.UTF8);
					}
				});
			});
		}
	}
}
