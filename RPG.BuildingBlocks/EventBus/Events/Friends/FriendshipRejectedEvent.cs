using System;

namespace RPG.BuildingBlocks.Common.EventBus.Events.Friends
{
    public class FriendshipRejectedEvent
    {
        public Guid ContentId { get; set; }
    }
}