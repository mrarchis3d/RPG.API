using System;

namespace RPG.BuildingBlocks.Common.EventBus.Events.Friends
{
    public class FriendshipRequestedEvent
    {
        public Guid ContentId { get; set; }
        public Guid UserRequester { get; set; }
        public Guid UserReceiver { get; set; }
    }
}
