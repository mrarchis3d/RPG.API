using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using RPG.BuildingBlocks.Common.StateManagement;

namespace RPG.BuildingBlocks.Common.Middlewares.Cache
{
    public class UseCacheMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IStateManagement _stateManagement;

        public UseCacheMiddleware(RequestDelegate next, IStateManagement stateManagement)
        {
            _next = next;
            _stateManagement = stateManagement;
        }

        public async Task Invoke(HttpContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }


            var endpoint = context.Features.Get<IEndpointFeature>()?.Endpoint;

            // Checking for known endpoints (Won't get hit if the URL didn't match an action (I.e. 404)
            if (endpoint == null)
            {
                await _next(context);
                return;
            }

            // Checking if a Cacheable Attribute exists
            ICacheableData cacheableData = endpoint.Metadata.GetMetadata<ICacheableData>();
            if (cacheableData == null)
            {
                await _next(context);
                return;

            }

            // If we got here, need to check the cache store
            string keyHash = await GetHashOfRequest(context.Request);

            SerializableResponse response = null;
            // Check for the return value in the cache store
            try
            {
                response = await _stateManagement.GetStateAsync<SerializableResponse>(cacheableData.StoreName, keyHash);
            }
            catch { /* Could not connect to the state store, executing normal response */ }


            // If value was found, rewrite the response
            if (response != null)
            {
                await RewriteResponseAsync(context, response);
                return;
            }

            // We got here, there was no response to rewrite
            // Executing and capturing response in SerializableResponse format
            response = await ExecuteNextAndRecord(context);

            // Storing with TTL
            await _stateManagement.SaveStateAsync(cacheableData.StoreName, keyHash, response, cacheableData.TtlInSeconds);
        }

        private async Task<string> GetHashOfRequest(HttpRequest request)
        {
            MD5 md5Hasher = MD5.Create();

            string body;
            using (StreamReader reader = new StreamReader(request.Body))
            {
                body = await reader.ReadToEndAsync();
            }

            string dataToHash = $"path: {request.Path}; verb: { request.Method }; querystring: { request.QueryString.Value }; body: { body }";
            byte[] data = md5Hasher.ComputeHash(Encoding.Default.GetBytes(dataToHash));

            return BitConverter.ToString(data);
        }

        private async Task RewriteResponseAsync(HttpContext context, SerializableResponse response)
        {
            context.Response.StatusCode = response.StatusCode;

            context.Response.Headers.Clear();
            foreach (var header in response.Headers)
            {
                context.Response.Headers.Add(header.Key, new Microsoft.Extensions.Primitives.StringValues(header.Value));
            }
            context.Response.Headers.Add("memcache", "true");

            await context.Response.WriteAsync(response.Body);
        }

        private async Task<SerializableResponse> ExecuteNextAndRecord(HttpContext context)
        {
            SerializableResponse response = new SerializableResponse();
            using (MemoryStream buffer = new MemoryStream())
            {
                // Replacing the response stream to MemoryStream to prevent its sending
                var originalStream = context.Response.Body;
                context.Response.Body = buffer;

                await _next(context);

                buffer.Seek(0, SeekOrigin.Begin);

                using (StreamReader bufferReader = new StreamReader(buffer))
                {
                    // Extracting the data from the replaced MemoryStream
                    response.Body = await bufferReader.ReadToEndAsync();
                    response.StatusCode = context.Response.StatusCode;
                    foreach (var header in context.Response.Headers)
                    {
                        response.Headers.Add(header.Key, header.Value.ToArray());
                    }


                    // Copying the data from the MemoryStream to the originalStream
                    context.Response.Body.Seek(0, SeekOrigin.Begin);
                    await context.Response.Body.CopyToAsync(originalStream);

                    // Appending the originalStream to the response body
                    context.Response.Body = originalStream;
                }
            }

            return response;
        }
    }
}
