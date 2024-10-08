﻿using RPG.BuildingBlocks.Common.Middlewares;
using Microsoft.AspNetCore.Builder;

namespace RPG.BuildingBlocks.Common.Extensions
{
    public static class MiddlewareExtensions
    {
        public static IApplicationBuilder UseHttpLoggingExtension(this IApplicationBuilder builder) 
            => builder.UseMiddleware<HttpLoggingMiddleware>();
    }
}
