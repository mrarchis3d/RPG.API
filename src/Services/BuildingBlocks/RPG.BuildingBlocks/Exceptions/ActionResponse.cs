using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace RPG.BuildingBlocks.Common.Exceptions
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