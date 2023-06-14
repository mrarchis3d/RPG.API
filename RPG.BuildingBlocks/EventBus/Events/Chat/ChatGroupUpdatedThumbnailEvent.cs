using System;

namespace RPG.BuildingBlocks.Common.EventBus.Events.Chat
{
    public class ChatGroupUpdatedThumbnailEvent
    {
        public Guid ChatId { get; set; }
        public string Cid { get; set; }
    }
}