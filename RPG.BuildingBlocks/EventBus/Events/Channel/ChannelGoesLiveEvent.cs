using System;

namespace RPG.BuildingBlocks.Common.EventBus.Events.Channel
{
    public class ChannelGoesLiveEvent
    {
        public Guid ChannelId { get; set; }
    }
}