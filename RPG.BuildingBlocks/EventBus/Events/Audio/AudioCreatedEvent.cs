using RPG.BuildingBlocks.Common.Enums;
using System;
using System.Collections.Generic;

namespace RPG.BuildingBlocks.Common.EventBus.Events.Audio
{
    public class AudioCreatedEvent
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public TargetType TargetType { get; set; }
        public Guid TargetId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string ThumbnailCid { get; set; }
        public List<string> Tags { get; set; }
        public List<Guid> Flags { get; set; }
        public Privacy Privacy { get; set; }
        public Guid CategoryId { get; set; }
        public string Cid { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string CategoryName { get; set; }
        public DateTime CreatedDate { get; set; }
        public string AudioStreamUrl { get; set; }
    }
}
