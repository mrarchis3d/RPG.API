using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Twilio;
using Twilio.Rest.Api.V2010.Account;

namespace RPG.BuildingBlocks.Common.SMS
{
    public class SmsMock : ISms
    {
        private readonly ILogger<SmsMock> _logger;

        public SmsMock(ILogger<SmsMock> logger)
        {
            _logger = logger;
        }

        public Task SendSMSAsync(string message, string toNumber)
        {
            _logger.LogInformation($"SendSMSAsync message: {message}, toNumber: {toNumber}");
            return Task.CompletedTask;
        }
    }
}
