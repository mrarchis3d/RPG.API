using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace RPG.BuildingBlocks.Common.Middlewares
{
    public class HttpLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<HttpLoggingMiddleware> _logger;
        public HttpLoggingMiddleware(RequestDelegate next, ILogger<HttpLoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await LogRequest(context.Request);
            }
            finally 
            {
                await _next(context);
            }
        }

        private async Task LogRequest(HttpRequest request)
        {
            request.EnableBuffering();

            var buffer = new byte[Convert.ToInt32(request.ContentLength)];

            await request.Body.ReadAsync(buffer, 0, buffer.Length);

            var bodyAsText = Encoding.UTF8.GetString(buffer);

            request.Body.Position = 0;
            var sb = new StringBuilder();
            sb.AppendLine($"Http Request Information:");
            sb.AppendLine($"Schema:{request.Scheme}");
            sb.AppendLine($"Host: {request.Host}");
            sb.AppendLine($"Path: {request.Path}");
            sb.AppendLine($"QueryString: {request.QueryString}");
            sb.AppendLine($"Headers: {GetHeaders(request.Headers)}");

            if (request.ContentType is null || !request.ContentType.Contains("application/octet-stream"))
            {
                sb.AppendLine($"Request Body: {bodyAsText}");
            }
            
            _logger.LogInformation(sb.ToString());
        }

        private async Task LogResponse(HttpResponse response)
        {
            //We need to read the response stream from the beginning...
            response.Body.Seek(0, SeekOrigin.Begin);

            //...and copy it into a string
            string responseBody = await new StreamReader(response.Body).ReadToEndAsync();

            //We need to reset the reader for the response so that the client can read it.
            response.Body.Position = 0;

            _logger.LogInformation($"Http Response Information:{Environment.NewLine}" +
                                    $"StatusCode: {response.StatusCode}" +
                                    $"Response Body: {responseBody} {Environment.NewLine}");
        }

        private string GetHeaders(IHeaderDictionary headers)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var key in headers.Keys)
            {
                sb.Append($"{key}: {headers[key]} {Environment.NewLine}");
            }

            return sb.ToString();
        }
    }
}
