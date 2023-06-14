using RPG.BuildingBlocks.Common.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace RPG.BuildingBlocks.Common.Middlewares
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;
        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (AppException e)
            {
                await WriteErrorResponse(httpContext, e);
            }
            catch (Exception e)
            {
                _logger.LogError("App middleware exception");
                _logger.LogError(e.ToString());
                await WriteErrorResponse(httpContext, e);
            }
        }

        private async Task WriteErrorResponse(HttpContext httpContext, Exception exception)
        {
            httpContext.Response.ContentType = "application/json";

            ActionResponse responseObj = null;
            if (exception is AppException)
            {
                responseObj = (exception as AppException).ActionResponse;
                httpContext.Response.StatusCode = (int)responseObj.StatusCode;
            }
            else if (exception is UnauthorizedAccessException)
            {
                responseObj = new ActionResponse()
                {
                    StatusCode = HttpStatusCode.Unauthorized,
                    Title = "The user does not have access to that resource"
                };

                httpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
            }
            else // Generic exception
            {
                httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                responseObj = new ActionResponse
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    Title = "An unexpected error has occurred, please try again later"
                };
            }

            await httpContext.Response.WriteAsync(JsonConvert.SerializeObject(responseObj));
        }
    }
}