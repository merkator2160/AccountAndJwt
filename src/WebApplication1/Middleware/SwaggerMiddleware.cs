using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.PlatformAbstractions;
using Swashbuckle.AspNetCore.Swagger;
using System.IO;
using System.Reflection;

namespace AccountAndJwt.AuthorizationService.Middleware
{
	internal static class SwaggerMiddleware
	{
		public static void AddConfiguredSwaggerGen(this IServiceCollection services)
		{
			var test = Assembly.GetEntryAssembly().GetCustomAttribute<AssemblyInformationalVersionAttribute>().InformationalVersion;

			services.AddSwaggerGen(c =>
			{
				c.SwaggerDoc("AccountAndJwt", new Info
				{
					Version = "v1",
					Title = "AccountAndJwt",
					Description = "AccountAndJwt API",
					TermsOfService = "None",
					Contact = new Contact()
					{
						Name = "Aleksandrov Evgeniy",
						Email = "ulthane2160@gmail.com"
					}
				});
				c.IncludeXmlComments(Path.Combine(PlatformServices.Default.Application.ApplicationBasePath, "Documentation.xml"));
				c.IgnoreObsoleteActions();
				c.IgnoreObsoleteProperties();
				c.DescribeAllEnumsAsStrings();
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
				c.SwaggerEndpoint("/swagger/AccountAndJwt.json", "AccountAndJwt API");
			});
		}
	}
}
