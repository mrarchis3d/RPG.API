using System;
using System.Collections.Generic;

namespace RPG.BuildingBlocks.Common.EventBus.Events.Chat
{
    public class OutgoingTextMessageEvent
    {
        public Guid Id { get; set; }
        public List<Guid> UsersTo { get; set; }
        public Guid ChatId { get; set; }
        public Guid UserFrom { get; set; }
        public string UserFromUsername { get; set; }
        public string UserFromFullname { get; set; }
        public string UserFromThumbnailCid { get; set; }
        public string Message { get; set; }
        public string Filefullname { get; set; }
        public string Filehash { get; set; }
        public string FileThumbnailCid { get; set; }
        public DateTime TimeStamp { get; set; }
        public string RequestId { get; set; }
    }
}