using RPG.BuildingBlocks.Common.Enums;
using System;
using System.Collections.Generic;

namespace RPG.BuildingBlocks.Common.EventBus.Events.Chat
{
    public class ChatGroupCreatedEvent
    {
        public Guid ChatId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsPrivate { get; set; }
        public string ThumbnailCid { get; set; }
        public List<string> Tags { get; set; }
        public ChatType Type { get; set; }
    }
}
