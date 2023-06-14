using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace RPG.BuildingBlocks.Common.ServiceDiscovery
{
    public interface IServiceDiscovery
    {
        Task<TOutput> InvokeMethodAsync<TInput, TOutput>(TInput data, string serviceName, string endPoint, HeaderDictionary headers = null, bool ignoreUser = false);

        Task InvokeMethodAsync<TInput>(TInput data, string serviceName, string endPoint);

        Task<TOutput> InvokeMethodAsync<TOutput>(HttpMethod method, string serviceName, string endPoint, HeaderDictionary headers = null);
        Task<TOutput> InvokeMethodAsync<TInput, TOutput>(HttpMethod method, TInput data, string serviceName, string endPoint);
        Task InvokeMethodAsync(HttpMethod method, string serviceName, string endPoint);

        Task<TOutput> InvokeMethodAsync<TOutput>(HttpMethod method, string serviceName, string endPoint, MultipartFormDataContent formData);
    }
}
