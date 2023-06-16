using System.Collections.Generic;

namespace RPG.BuildingBlocks.Common.Middlewares.Cache
{
    public class SerializableResponse
    {
        public string Body { get; set; }
        public int StatusCode { get; set; }
        public Dictionary<string, string[]> Headers { get; set; } = new Dictionary<string, string[]>();

    }
}
