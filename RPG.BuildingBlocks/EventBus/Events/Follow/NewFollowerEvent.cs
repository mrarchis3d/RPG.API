using System;

namespace RPG.BuildingBlocks.Common.EventBus.Events.Follow
{
    public class NewFollowerEvent
    {
        public Guid FollowedId { get; set; }
        public Guid FollowerId { get; set; }
        public Guid FollowId { get; set; }
        public string followedUserName { get; set; }
        public string followedUserFullName { get; set; }
    }
}