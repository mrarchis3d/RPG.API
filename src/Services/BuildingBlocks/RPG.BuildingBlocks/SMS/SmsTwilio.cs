using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Twilio;
using Twilio.Rest.Api.V2010.Account;

namespace RPG.BuildingBlocks.Common.SMS
{
    public class SmsTwilio : ISms
    {
        private IConfiguration _config;
        private readonly ILogger<SmsTwilio> _logger;

        public SmsTwilio(IConfiguration config, ILogger<SmsTwilio> logger)
        {
            _config = config;
            _logger = logger;
        }

        public async Task SendSMSAsync(string message, string toNumber)
        {
            var accountSid = _config["sms_account_sid"];
            var authToken = _config["sms_auth_token"];
            var fromNumber = _config["sms_from_number"];

            TwilioClient.Init(accountSid, authToken);

            var result = await MessageResource.CreateAsync(
                body: message,
                from: new Twilio.Types.PhoneNumber(fromNumber),
                to: new Twilio.Types.PhoneNumber($"+{toNumber}")
            );
        }
    }
}
