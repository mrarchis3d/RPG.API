using System;

namespace RPG.BuildingBlocks.Common.EventBus.Events.Friends
{
    public class FriendshipCanceledEvent
    {
        public Guid ContentId { get; set; }
    }
}