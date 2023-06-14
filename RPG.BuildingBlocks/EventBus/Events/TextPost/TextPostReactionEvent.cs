using RPG.BuildingBlocks.Common.Enums;
using Ourglass.Reactions.Domain.Enums;
using System;
using System.Collections.Generic;

namespace RPG.BuildingBlocks.Common.EventBus.Events.TextPost
{
    public class TextPostReactionEvent
    {
        public Guid Id { get; set; }
        public string Text { get; set; }
        public string UserId { get; set; }
        public TargetType TargetType { get; set; }
        public Guid TargetId { get; set; }
        public Privacy Privacy { get; set; }
        public DateTime CreatedDate { get; set; }
        public Guid ReactedUserId { get; set; }
        public ReactionType ReactionType { get; set; }
        public List<FileInformation> Files { get; set; }
    }

    public class FileInformation
    {
        public string Cid { get; set; }
        public string FileExtension { get; set; }
        public long SizeMb { get; set; }
        public string ContentType { get; set; }
        public string Filehash { get; set; }
        public string FileName { get; set; }
    }

}
