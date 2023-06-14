using System;

namespace RPG.BuildingBlocks.Common.EventBus.Events.Circle
{
    public class CircleUpdatedThumbnailEvent
    {
        public Guid CircleId { get; set; }
        public string Cid { get; set; }
    }
}
