using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.PlatformAbstractions;
using Microsoft.OpenApi.Models;
using System;
using System.IO;
using System.Reflection;

namespace AccountAndJwt.AuthorizationService.Middleware
{
	internal static class SwaggerMiddleware
	{
		private const String _documentName = "AccountAndJwt";


		public static void AddConfiguredSwaggerGen(this IServiceCollection services)
		{
			var assemblyVersion = Assembly.GetExecutingAssembly().GetName().Version;

			services.AddSwaggerGen(c =>
			{
				c.SwaggerDoc(_documentName, new OpenApiInfo
				{
					Version = $"v{assemblyVersion}",
					Title = "Monitor service",
					Description = "Monitor API",
					Contact = new OpenApiContact()
					{
						Name = "Aleksandrov Evgeniy",
						Email = "ulthane2160@gmail.com",
						Url = new Uri("https://www.linkedin.com/in/evgeniy-alexandrov-967388100")
					}
				});
				c.IncludeXmlComments(Path.Combine(PlatformServices.Default.Application.ApplicationBasePath, "Documentation.xml"));
				c.IgnoreObsoleteActions();
				c.IgnoreObsoleteProperties();
			});
		}
		public static void UseConfiguredSwagger(this IApplicationBuilder app)
		{
			app.UseSwagger(c =>
			{
				c.RouteTemplate = "swagger/{documentName}.json";
			});
			app.UseSwaggerUI(c =>
			{
				c.SwaggerEndpoint($"/swagger/{_documentName}.json", "Account and JWT API");
			});
		}
	}
}
