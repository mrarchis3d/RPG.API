using System;

namespace RPG.BuildingBlocks.Common.EventBus.Events.PostVideo
{
    public class PostVideoUpdatedThumbnailEvent
    {
        public Guid PostVideoId { get; set; }
        public string Cid { get; set; }
    }
}
