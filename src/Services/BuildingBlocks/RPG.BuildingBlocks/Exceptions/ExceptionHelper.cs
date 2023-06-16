using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace RPG.BuildingBlocks.Common.Exceptions
{
    public static class ExceptionHelper
    {
        public static async Task ErrorHandle(HttpResponseMessage httpResponse, ILogger logger)
        {
            switch (httpResponse.StatusCode)
            {
                case HttpStatusCode.BadRequest:
                    var badReq = JsonConvert.DeserializeObject<ActionResponse>(await httpResponse.Content.ReadAsStringAsync());
                    throw new AppException(badReq.Title);
                case HttpStatusCode.Unauthorized:
                    throw new AppException(httpResponse.Headers.WwwAuthenticate.ToString(), httpResponse.StatusCode);
                default:
                    var body = await httpResponse.Content.ReadAsStringAsync() ?? "";
                    logger.LogError($"Status Code: {httpResponse.StatusCode} - Content: {body}");
                    throw new AppException(httpResponse.StatusCode);
            }
        }
    }
}
