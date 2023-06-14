using System;
using System.Collections.Generic;

namespace RPG.BuildingBlocks.Common.EventBus.Events.PushNotifications
{
    public class PushNotificationCreated
    {
        public List<Guid> UsersId { get; set; }
        public TemplateEnum Template { get; set; }
        public Dictionary<string, string> TemplateData { get; set; }
        public Dictionary<string, string> Data { get; set; }
        public string Title { get; set; }
        public bool Silent { get; set; } = false;
    }
    /// <summary>
    /// TEMPLATE should follow the rule: {APPNAME}_{ACTION CAN BE SEPARATED BY _ ONLY THE FIRST ONE MATTERS}
    /// </summary>
    public enum TemplateEnum
    {
        CHAT_NEW_MESSAGE = 1,
        CHAT_JOIN_GROUP = 2,
        NOTIFICATIONMANAGER_COUNT_UPDATED = 3,
        CHAT_INCOMING_CALL = 4,
        CHAT_NOTIFICATION_RECEIVED = 5,
        TEST = 999
    }
}
