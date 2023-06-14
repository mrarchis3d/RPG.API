using System;
namespace RPG.BuildingBlocks.Common.EventBus.Events.LiveConcert
{
    public class LiveConcertUpdatedThumbnailEvent
    {
        public Guid ConcertId { get; set; }
        public string Cid { get; set; }
    }
}
