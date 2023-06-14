using System;
using System.Collections.Generic;

namespace RPG.BuildingBlocks.Common.EventBus.Events.Chat
{
    public class ChatSystemMessageEvent
    {
        public List<ChatSystemMessageDto> SystemInteractions { get; set; }
    }

    public class ChatSystemMessageDto
    {
        public Guid Id { get; set; }
        public Guid ChatId { get; set; }
        public string Body { get; set; }
        public SystemMessageType MessageType { get; set; }
        public Guid Recipient { get; set; }
        public DateTime Timestamp { get; set; }
    }

    public enum SystemMessageType
    {
        JOINED = 0,
        REMOVED = 1,
        LAST_ADMIN_REMOVED = 2,
        CHANGE_ENCRYPTION_KEY = 3
    }
}
