using Newtonsoft.Json;
using System.Net;

namespace RPGAPI.Utils
{
    public class ActionResponse
    {
        [JsonProperty("statusCode")]
        public HttpStatusCode StatusCode { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }
    }
}
