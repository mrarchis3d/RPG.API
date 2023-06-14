using System;

namespace RPG.BuildingBlocks.Common.EventBus.Events.Chat
{
    public class IncommingTextMessageEvent 
    {
        public Guid UserFrom { get; set; }
        public Guid ChatId { get; set; }
        public string Message { get; set; }
        public DateTime TimeStamp { get; set; }
        public string Filename { get; set; }
        public string Fileext { get; set; }
        public string Filehash { get; set; }
        public string ThumbnailCid { get; set; }
        public string RequestId { get; set; }
    }
}
