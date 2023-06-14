using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace RPG.BuildingBlocks.Common.CommonLog
{
    public class CentralizedLogger
    {
        private readonly ILogger<CentralizedLogger> _logger;

        public CentralizedLogger(ILogger<CentralizedLogger> logger)
        {
            _logger = logger;
        }

        public void DoLog(dynamic obj)
        {
            _logger.LogInformation($"{JsonConvert.SerializeObject(obj)}");
        }
    }
}
