using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dapr.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace RPG.BuildingBlocks.Common.Email
{
    public class EmailDapr : IEmail
    {
        private IConfiguration _config;
        private readonly ILogger<EmailDapr> _logger;
        private readonly DaprClient _daprClient;

        public EmailDapr(IConfiguration config, ILogger<EmailDapr> logger, DaprClient daprClient)
        {
            _config = config;
            _logger = logger;
            _daprClient = daprClient;
        }

        public async Task SendEmailAsync(string emailTo, string subject, string body)
        {
            var smtpFrom = _config["smtp_from"];

            var metadata = new Dictionary<string, string>
            {
                ["emailFrom"] = smtpFrom,
                ["emailTo"] = emailTo,
                ["subject"] = subject
            };

            await _daprClient.InvokeBindingAsync("smtp", "create",
                body, metadata);
        }
    }
}
