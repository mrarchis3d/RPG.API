using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dapr.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace RPG.BuildingBlocks.Common.Email
{
    public class EmailMock : IEmail
    {
        private readonly ILogger<EmailMock> _logger;

        public EmailMock(ILogger<EmailMock> logger)
        {
            _logger = logger;
        }

        public Task SendEmailAsync(string emailTo, string subject, string body)
        {
            _logger.LogInformation($"SendEmailAsync emailTo: {emailTo}, subject: {subject}, body: {body}");
            return Task.CompletedTask;
        }
    }
}
