using System;

namespace RPG.BuildingBlocks.Common.EventBus.Events.Circle
{
    public class CircleChatCreatedEvent
    {
        public Guid CircleId { get; set; }
        public Guid ChatId { get; set; }
    }
}
