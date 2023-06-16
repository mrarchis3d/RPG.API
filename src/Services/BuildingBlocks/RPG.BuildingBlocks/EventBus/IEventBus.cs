using System.Threading.Tasks;

namespace RPG.BuildingBlocks.Common.EventBus
{
    public interface IEventBus
    {
        Task PublishEventAsync<T>(T @event, string componentName, string topicName);
    }
}
