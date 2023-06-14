using System;
namespace RPG.BuildingBlocks.Common.EventBus.Events.Circle
{
    public class CirclePrivacyUpdatedEvent
    {
        public Guid CircleId { get; set; }
        public bool IsPrivate { get; set; }
    }
}
