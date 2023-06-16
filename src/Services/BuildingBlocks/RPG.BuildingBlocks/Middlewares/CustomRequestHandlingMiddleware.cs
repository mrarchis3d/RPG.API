using RPG.BuildingBlocks.Common.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPG.BuildingBlocks.Common.Middlewares
{
    public class CustomRequestHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<CustomRequestHandlingMiddleware> _logger;
        public CustomRequestHandlingMiddleware(RequestDelegate next, ILogger<CustomRequestHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context, CustomRequestInfo customRequestInfo)
        {
            var rqf = context.Request.HttpContext.Features.Get<IRequestCultureFeature>();
            var culture = rqf.RequestCulture.Culture;

            customRequestInfo.Language = culture.TwoLetterISOLanguageName;

            if (culture.TwoLetterISOLanguageName != "en" && culture.TwoLetterISOLanguageName != "es")
            {
                customRequestInfo.Language = "en";
            }

            await _next.Invoke(context);
        }
    }
}