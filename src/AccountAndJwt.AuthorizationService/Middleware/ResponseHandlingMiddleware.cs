using AccountAndJwt.Common.Const;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;

namespace AccountAndJwt.AuthorizationService.Middleware
{
	internal static class ResponseHandlingMiddleware
	{
		public static void ConfigureResponseHandling(this IServiceCollection services)
		{
			services.AddResponseCompression(options =>
			{
				options.Providers.Add<BrotliCompressionProvider>();
				options.Providers.Add<GzipCompressionProvider>();

				options.EnableForHttps = true;
				options.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(new[] { HttpMimeType.Image.SvgXml, HttpMimeType.Application.Javascript });
			});
		}
	}
}