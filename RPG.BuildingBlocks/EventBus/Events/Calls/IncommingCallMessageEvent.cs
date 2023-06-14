using System;

namespace RPG.BuildingBlocks.Common.EventBus.Events.Calls
{
    public class InitiateCallEvent 
    {
        public Guid UserTo { get; set; }
        public Guid UserFrom { get; set; }
        public string UserNameFrom { get; set; }
        public string FullNameFrom { get; set; }
        public string ThumbnailCidFrom { get; set; }
        public string Metadata { get; set; }
        public DateTime TimeStamp { get; set; }
    }
}
