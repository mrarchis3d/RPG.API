using System.Threading.Tasks;
using Dapr.Client;
using Microsoft.Extensions.Logging;

namespace RPG.BuildingBlocks.Common.EventBus
{
    public class EventBusMock : IEventBus
    {
        private readonly ILogger<EventBusMock> _logger;

        public EventBusMock(ILogger<EventBusMock> logger)
        {
            _logger = logger;
        }

        public Task PublishEventAsync<T>(T @event, string componentName, string topicName)
        {
            _logger.LogInformation($"PublishEventAsync componentName: {componentName}, topicName: {topicName}");
            return Task.CompletedTask;
        }
    }
}
