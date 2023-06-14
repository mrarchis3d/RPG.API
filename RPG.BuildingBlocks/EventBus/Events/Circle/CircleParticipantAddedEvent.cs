using System;

namespace RPG.BuildingBlocks.Common.EventBus.Events.Circle
{
    public class CircleParticipantAddedEvent
    {
        public Guid Id { get; set; }
        public Guid CircleId { get; set; }
        public Guid UserId { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string OwnerEncryptionKey { get; set; }
        public bool CircleIsPrivate { get; set; }
    }
}
