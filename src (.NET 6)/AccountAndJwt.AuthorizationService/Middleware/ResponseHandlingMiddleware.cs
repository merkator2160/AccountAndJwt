﻿using AccountAndJwt.Contracts.Const;
using Microsoft.AspNetCore.ResponseCompression;

namespace AccountAndJwt.AuthorizationService.Middleware
{
    internal static class ResponseHandlingMiddleware
    {
        public static void AddResponseHandling(this IServiceCollection services)
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