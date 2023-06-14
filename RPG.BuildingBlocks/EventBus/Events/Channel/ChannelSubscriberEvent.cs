using System;

namespace RPG.BuildingBlocks.Common.EventBus.Events.Channel
{
    public class ChannelSubscriberEvent
    {
        public Guid SubscriptionId { get; set; }
        public Guid ChannelId { get; set; }
        public Guid UserId { get; set; }
        public string shortName { get; set; }
        public string longName { get; set; }
    }
}