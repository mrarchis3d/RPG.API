using System;
using RPG.BuildingBlocks.Common.Enums;

namespace RPG.BuildingBlocks.Common.EventBus.Events.Calls
{
    public class NotifyCallEvent 
    {
        public Guid UserTo { get; set; }
        public Guid UserFrom { get; set; }
        public string Metadata { get; set; }
        public string UserNameFrom { get; set; }
        public string FullNameFrom { get; set; }
        public CallEventType CallEventType { get; set; }
        public DateTime TimeStamp { get; set; }
    }
}
