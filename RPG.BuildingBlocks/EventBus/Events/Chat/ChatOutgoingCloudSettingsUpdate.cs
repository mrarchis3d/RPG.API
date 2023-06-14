using System;

namespace RPG.BuildingBlocks.Common.EventBus.Events.User
{
    public class ChatOutgoingCloudSettingsUpdate
    {
        public Guid UserId { get; set; }
        public string Settings { get; set; }
    }
}