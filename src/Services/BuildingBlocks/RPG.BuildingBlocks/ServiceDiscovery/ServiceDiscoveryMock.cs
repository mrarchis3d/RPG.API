using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace RPG.BuildingBlocks.Common.ServiceDiscovery
{
    public class ServiceDiscoveryMock : IServiceDiscovery
    {
        private readonly ILogger<ServiceDiscoveryMock> _logger;

        public ServiceDiscoveryMock(ILogger<ServiceDiscoveryMock> logger)
        {
            _logger = logger;
        }

        public Task<TOutput> InvokeMethodAsync<TInput, TOutput>(TInput data, string serviceName, string endPoint, HeaderDictionary header = null, bool ignoreUser = false)
        {
            _logger.LogInformation($"InvokeMethodAsync serviceName: {serviceName}, endPoint: {endPoint}");

            TOutput obj = (TOutput)Activator.CreateInstance(typeof(TOutput));
            return Task.FromResult(obj);
        }

        public Task InvokeMethodAsync<TInput>(TInput data, string serviceName, string endPoint)
        {
            _logger.LogInformation($"InvokeMethodAsync serviceName: {serviceName}, endPoint: {endPoint}");
            return Task.CompletedTask;
        }

        public Task<TOutput> InvokeMethodAsync<TOutput>(HttpMethod method, string serviceName, string endPoint, HeaderDictionary header = null)
        {
            _logger.LogInformation($"InvokeMethodAsync serviceName: {serviceName}, endPoint: {endPoint}");

            TOutput obj = (TOutput)Activator.CreateInstance(typeof(TOutput));
            return Task.FromResult(obj);
        }

        public Task<TOutput> InvokeMethodAsync<TInput, TOutput>(HttpMethod method, TInput data, string serviceName, string endPoint)
        {
            _logger.LogInformation($"InvokeMethodAsync serviceName: {serviceName}, endPoint: {endPoint}");

            TOutput obj = (TOutput)Activator.CreateInstance(typeof(TOutput));
            return Task.FromResult(obj);
        }

        public Task InvokeMethodAsync(HttpMethod method, string serviceName, string endPoint)
        {
            _logger.LogInformation($"InvokeMethodAsync serviceName: {serviceName}, endPoint: {endPoint}, method: {method.ToString()}");
            return Task.CompletedTask;
        }

        public Task<TOutput> InvokeMethodAsync<TOutput>(HttpMethod method, string serviceName, string endPoint, MultipartFormDataContent formData)
        {
            _logger.LogInformation($"InvokeMethodAsync serviceName: {serviceName}, endPoint: {endPoint}, method: {method.ToString()}");
            TOutput obj = (TOutput)Activator.CreateInstance(typeof(TOutput));
            return Task.FromResult(obj);
        }
    }
}
