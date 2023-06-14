using System;

namespace RPG.BuildingBlocks.Common.EventBus.Events.Chat
{
    public class ChatIncommingCloudSettingsUpdate
    {
        public Guid UserId { get; set; }
        public string Settings { get; set; }
    }
}