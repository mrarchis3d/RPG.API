using System;
namespace RPG.BuildingBlocks.Common.EventBus.Events.Channel
{
    public class ChannelUpdatedThumbnailEvent
    {
        public Guid ChannelId { get; set; }
        public string Cid { get; set; }
    }
}
