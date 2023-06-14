using RPG.BuildingBlocks.Common.Enums;
using System;
using System.Collections.Generic;

namespace RPG.BuildingBlocks.Common.EventBus.Events.TextPost
{
    public class TextPostCreatedEvent
    {
        public string Id { get; set; }
        public List<FileInfo> Files { get; set; }
        public string Text { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string UserThumbnailCid { get; set; }
        public TargetType TargetType { get; set; }
        public Guid TargetId { get; set; }
        public Privacy Privacy { get; set; }
        public DateTime CreatedDate { get; set; }
    }

    public class FileInfo
    {
        public string Cid { get; set; }
        public string FileExtension { get; set; }
        public long SizeMb { get; set; }
        public string ContentType { get; set; }
        public string Filehash { get; set; }
        public string FileName { get; set; }
    }
}
