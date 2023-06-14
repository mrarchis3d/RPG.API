using System;

namespace RPG.BuildingBlocks.Common.EventBus.Events.Circle
{
    public class CircleCreatedEvent
    {
        public Guid CircleId { get; set; }
        public Guid UserId { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string Name { get; set; }
        public bool IsPrivate { get; set; }
        public bool Visibility { get; set; }
        public string OwnerEncryptionKey { get; set; }
    }
}
