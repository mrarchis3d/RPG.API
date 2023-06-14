using System;
using System.Net.Http;
using System.Threading.Tasks;
using RPG.BuildingBlocks.Common.Extensions;
using RPG.BuildingBlocks.Common.ServiceDiscovery;
using RPG.BuildingBlocks.Common.SharedServices.Identity.Dto;
using Dapr.Client;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;

namespace RPG.BuildingBlocks.Common.Providers.Identity
{
    public interface IIdentityProvider
    {
        Task<UserInfoDto> GetUserInfoAsync();
        Task<UserInfoDto> GetUserInfoAsync(string userId);
        Task<Guid> GetUserId();
    }

    public class IdentityProvider : IIdentityProvider
    {
        private readonly HttpClient _client;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IServiceDiscovery _serviceDiscovery;
        private readonly ILogger<IdentityProvider> _logger;

        public IdentityProvider(HttpClient client, IConfiguration configuration, IHttpContextAccessor contextAccessor, IServiceDiscovery serviceDiscovery, ILogger<IdentityProvider> logger)
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));
            _configuration = configuration;
            _contextAccessor = contextAccessor;
            _serviceDiscovery = serviceDiscovery;
            _logger = logger;
        }


        /// <summary>
        /// This function requires that the authorization is injected as a header AND IT MUST BE A VALID USER (DO NOT USE EventBus Auth)
        /// </summary>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        private async Task<GetIdentityInfoResponse> QueryUserInformationByBearerAsync()
        {
            var daprClient = new DaprClientBuilder().Build();

            if (daprClient is null)
                throw new ArgumentException("Identity service not found");

            if (_client.DefaultRequestHeaders.Authorization is null)
                throw new UnauthorizedAccessException();

            HttpResponseMessage response;
            var endpoint = string.Empty;

            if (_client.DefaultRequestHeaders.Contains("X-USER-ID"))
            {
                endpoint = "/api/v1/profile/internal/userinfo";
            }
            else
            {
                endpoint = "/connect/userinfo";
            }
            ;
            response = await _client.GetAsync(daprClient.CreateInvokeMethodRequest(HttpMethod.Get, "identity", endpoint).RequestUri);

            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadAsStringAsync();

            var jsonResult =
                JsonConvert.DeserializeObject<GetIdentityInfoResponse>(result);

            return jsonResult;
        }


        /// <summary>
        /// This function requires that the authorization is injected as a header AND IT MUST BE A VALID USER (DO NOT USE EventBus Auth)
        /// </summary>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        private async Task<UserInfoDto> QueryUserInformationByBearerAsyncEx()
        {
            var userId = await _contextAccessor.HttpContext.GetUserId();
            _logger.LogInformation($"UserId: {userId}");
            return await GetUserInfoAsync(userId);
        }

        public async Task<UserInfoDto> GetUserInfoAsync(string userId)
        {
            var response = await _serviceDiscovery.InvokeMethodAsync<UserInfoDto>(HttpMethod.Get, "identity",
                "/api/v1/profile/internal/userinfo", new HeaderDictionary()
                {
                    {"X-USER-ID", new StringValues(userId)}
                });
            _logger.LogInformation($"raw response : {JsonConvert.SerializeObject(response)}");

            return response;
        }
        public async Task<UserInfoDto> GetUserInfoAsync()
        {
            var req = await QueryUserInformationByBearerAsyncEx();
            return req;
            //return new UserInfoDto() {UserId = req.Sub, Username = req.Preferred_Username, Fullname = req.Name};
        }
        public async Task<Guid> GetUserId()
        {
            return Guid.Parse(await _contextAccessor.HttpContext.GetUserId());
        }
        public record GetIdentityInfoResponse(Guid Sub, string Name, string Preferred_Username);

    }
}
