using System;

namespace RPG.BuildingBlocks.Common.EventBus.Events.PostVideo
{
    public class VideoTranscodedEvent
    {
        public Guid VideoId { get; set; }
    }
}