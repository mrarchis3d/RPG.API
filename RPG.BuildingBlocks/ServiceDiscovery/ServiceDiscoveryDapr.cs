using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using RPG.BuildingBlocks.Common.Exceptions;
using RPG.BuildingBlocks.Common.Extensions;
using Dapr.Client;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace RPG.BuildingBlocks.Common.ServiceDiscovery
{
    public class ServiceDiscoveryDapr : IServiceDiscovery
    {
        private DaprClient _daprClient;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<ServiceDiscoveryDapr> _logger;

        public ServiceDiscoveryDapr(DaprClient daprClient, IHttpContextAccessor httpContextAccessor, ILogger<ServiceDiscoveryDapr> logger)
        {
            _daprClient = daprClient;
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
        }


        public async Task InvokeMethodAsync<TInput>(TInput data, string serviceName, string endPoint)
        {
            var request = _daprClient.CreateInvokeMethodRequest(serviceName, endPoint, data);

            await AddUserHeader(request);

            await _daprClient.InvokeMethodAsync(request);

        }

        public async Task InvokeMethodAsync(HttpMethod method, string serviceName, string endPoint)
        {
            var request = _daprClient.CreateInvokeMethodRequest(method, serviceName, endPoint);

            await AddUserHeader(request);

            await _daprClient.InvokeMethodAsync(request);
        }

        public async Task<TOutput> InvokeMethodAsync<TInput, TOutput>(TInput data, string serviceName, string endPoint, HeaderDictionary headers = null, bool ignoreUser = false)
        {
            var request = _daprClient.CreateInvokeMethodRequest(serviceName, endPoint, data);

            if (headers != null)
            {
                foreach (var header in headers)
                {
                    request.Headers.Add(header.Key, header.Value.ToArray());
                }
            }

            if (!ignoreUser)
                await AddUserHeader(request);


            var result = await _daprClient.InvokeMethodWithResponseAsync(request);

            return await HandleResponse<TOutput>(serviceName, endPoint, result);
        }

        public async Task<TOutput> InvokeMethodAsync<TOutput>(HttpMethod method, string serviceName, string endPoint, HeaderDictionary headers = null)
        {
            var request = _daprClient.CreateInvokeMethodRequest(method, serviceName, endPoint);

            if (headers != null)
            {
                foreach (var header in headers)
                {
                    request.Headers.Add(header.Key, header.Value.ToArray());
                }
            }

            await AddUserHeader(request);


            var result = await _daprClient.InvokeMethodWithResponseAsync(request);

            return await HandleResponse<TOutput>(serviceName, endPoint, result);
        }

        public async Task<TOutput> InvokeMethodAsync<TInput, TOutput>(HttpMethod method, TInput data, string serviceName, string endPoint)
        {
            var request = _daprClient.CreateInvokeMethodRequest(method, serviceName, endPoint, data);

            if (method == HttpMethod.Patch)
            {
                request.Content = new StringContent(JsonConvert.SerializeObject(data));
                request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json-patch+json");
            }

            await AddUserHeader(request);

            var result = await _daprClient.InvokeMethodWithResponseAsync(request);

            return await HandleResponse<TOutput>(serviceName, endPoint, result);
        }

        public async Task<TOutput> InvokeMethodAsync<TOutput>(HttpMethod method, string serviceName, string endPoint, MultipartFormDataContent formData)
        {
            var request = _daprClient.CreateInvokeMethodRequest(method, serviceName, endPoint);
            request.Content = formData;
            await AddUserHeader(request);
            var result = await _daprClient.InvokeMethodWithResponseAsync(request);
            return await HandleResponse<TOutput>(serviceName, endPoint, result);
        }
        
        private async Task<TOutput> HandleResponse<TOutput>(string serviceName, string endPoint,
            HttpResponseMessage result)
        {
            if (result.IsSuccessStatusCode)
            {
                var response = await result.Content.ReadAsStringAsync();

                try
                {
                    if (!string.IsNullOrEmpty(response))
                        return JsonConvert.DeserializeObject<TOutput>(response);
                }
                catch (Exception e)
                {
                    _logger.LogError(e, e.Message);
                    return default!;
                }

                return default!;
            }

            throw new InvocationException(serviceName, endPoint,
                new AppException("Server did not respond as expected", result.StatusCode), result.EnsureSuccessStatusCode());
        }

        private async Task AddUserHeader(HttpRequestMessage request)
        {
            var userId = await _httpContextAccessor.HttpContext.GetUserId();

            if (userId is null)
            {
                return;
            }

            if (!request.Headers.Contains("X-USER-ID"))
            {
                request.Headers.Add("X-USER-ID", new string[] { userId });
            }

        }
    }
}
