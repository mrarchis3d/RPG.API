using System;
namespace RPG.BuildingBlocks.Common.EventBus.Events.LiveConcertTier
{
    public class LiveConcertTierUpdatedThumbnailEvent
    {
        public Guid LiveConcertTierId { get; set; }
        public string Cid { get; set; }
    }
}
