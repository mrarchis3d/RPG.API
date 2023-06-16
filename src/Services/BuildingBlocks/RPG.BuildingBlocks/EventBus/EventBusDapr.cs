using System.Threading.Tasks;
using Dapr.Client;

namespace RPG.BuildingBlocks.Common.EventBus
{
    public class EventBusDapr : IEventBus
    {
        private DaprClient _daprClient;

        public EventBusDapr(DaprClient daprClient)
        {
            _daprClient = daprClient;
        }

        public async Task PublishEventAsync<T>(T @event, string componentName, string topicName)
        {
            try
            {
                await _daprClient.PublishEventAsync(componentName, topicName, @event);
            }
            catch
            {
                throw;
            }
        }
    }
}
