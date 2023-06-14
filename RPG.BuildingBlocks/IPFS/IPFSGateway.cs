using RPG.BuildingBlocks.Common.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Polly;
using Polly.Retry;
using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace RPG.BuildingBlocks.Common.IPFS
{
    public class IPFSGateway : IIPFS
    {
        private readonly ILogger<IPFSGateway> _logger;
        private readonly HttpClient _ipfsClusterClient;
        private readonly HttpClient _ipfsClient;
        private AsyncRetryPolicy retryPolicy;
        private const int MAX_RETRIES = 3;
        private TimeSpan pauseBetweenFailures = TimeSpan.FromSeconds(10);

        public IPFSGateway(IHttpClientFactory clientFactory, ILogger<IPFSGateway> logger)
        {
            _logger = logger;
            _ipfsClusterClient = clientFactory.CreateClient("ipfscluster");
            _ipfsClient = clientFactory.CreateClient("ipfs");
            retryPolicy = Policy.Handle<HttpRequestException>()
                                .WaitAndRetryAsync(MAX_RETRIES, i => pauseBetweenFailures);
        }

        
        public async Task<Stream> GetFile(string cid)
        {
            string url = $"ipfs/{cid}";            
            HttpResponseMessage response = new HttpResponseMessage();
            await retryPolicy.ExecuteAsync(async () =>
            {
               response = await _ipfsClient.GetAsync(url);
            });

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsStreamAsync();
            }
            else
            {
                throw new AppException(response.ReasonPhrase);
            }
        }    
        public async Task<string> UploadFileToIpfsCluster(IFormFile file)
        {
            //need to check the size of the file to know which client to use to upload. 5mb or more is considered large file. 
            //var url = file.Length > 4500000? "ipfs/v0/add": "add";
            string url = "add";
            var fileOpen = file.OpenReadStream();
            _logger.LogInformation($"length file open ${fileOpen.Length}");
            
            using (var content = new StreamContent(fileOpen))
            using (var form = new MultipartFormDataContent())
            {
                content.Headers.ContentLength = file.Length;
                content.Headers.ContentType = MediaTypeHeaderValue.Parse(file.ContentType);
                form.Add(content, "file", file.FileName);                
                HttpResponseMessage response = new HttpResponseMessage();
                await retryPolicy.ExecuteAsync(async () =>
                {
                    response = await _ipfsClusterClient.PostAsync(url, form);
                });
                
                if (response.IsSuccessStatusCode)
                {
                    string responseContent = await response.Content.ReadAsStringAsync();
                    _logger.LogInformation($"response from APIFS uploading the file ${responseContent}");
                    return ParseSpecialIpfsClusterResponseData(responseContent);
                }
                else
                {
                    _logger.LogError($"An Error happened uploading file to IPFS {response.ReasonPhrase}");
                    throw new AppException(response.ReasonPhrase);
                }
            }
        }

        private string ParseSpecialIpfsClusterResponseData(object objectResponse)
        {
            _logger.LogInformation("IPFS Response");
            _logger.LogInformation(JsonConvert.SerializeObject(objectResponse));
            _logger.LogInformation("end IPFS Response");
            dynamic responseJsonObject =
                objectResponse.GetType().Equals(typeof(string)) ? JObject.Parse((string)objectResponse) :
                objectResponse;
            return responseJsonObject?.cid["/"];
        }
    }
}
