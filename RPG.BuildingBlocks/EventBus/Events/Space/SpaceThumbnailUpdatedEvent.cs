using System;

namespace RPG.BuildingBlocks.Common.EventBus.Events.Space
{
    public class SpaceThumbnailUpdatedEvent
    {
        public Guid SpaceId { get; set; }
        public string ThumbnailCid { get; set; }
        public string CoverCid { get; set; }
    }

}
