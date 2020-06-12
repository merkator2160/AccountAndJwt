using AccountAndJwt.Database.Models.Storage;
using Microsoft.AspNet.OData.Builder;
using Microsoft.AspNet.OData.Formatter;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using Microsoft.OData.Edm;
using System;
using System.Linq;

namespace AccountAndJwt.AuthorizationService.Middleware
{
	internal static class OdataMiddleware
	{
		public static IEdmModel CreateEdmModel(this IServiceProvider serviceProvider)
		{
			var builder = new ODataConventionModelBuilder(serviceProvider);
			builder.EntitySet<ValueDb>("Values");

			return builder.GetEdmModel();
		}

		/// <summary>
		/// ODate endpoints fix for Swagger
		/// Workaround: https://github.com/OData/WebApi/issues/1177
		/// </summary>
		public static void AddOdataMediaTypes(this MvcOptions options)
		{
			foreach(var outputFormatter in options.OutputFormatters.OfType<ODataOutputFormatter>().Where(_ => _.SupportedMediaTypes.Count == 0))
			{
				outputFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue("application/prs.odatatestxx-odata"));
			}
			foreach(var inputFormatter in options.InputFormatters.OfType<ODataInputFormatter>().Where(_ => _.SupportedMediaTypes.Count == 0))
			{
				inputFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue("application/prs.odatatestxx-odata"));
			}
		}
	}
}