using System;
using System.Collections.Generic;
using RPG.BuildingBlocks.Common.Enums;

namespace RPG.BuildingBlocks.Common.EventBus.Events.PostVideo
{
    public enum PostVideoStatusEnum : int
    {
        Processing = 0,
        Review = 1,
        Reviewed = 2
    }

    public class PostVideoCreatedEvent
    {
        public Guid VideoId { get; set; }
        public Guid UserId { get; set; }
        public Guid ChannelId { get; set; }
        public TargetType TargetType { get; set; }
        public Guid TargetId { get; set; }
        public string Fullname { get; set; }
        public string Username { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string ThumbnailCid { get; set; }
        public string Tags { get; set; }
        public Privacy Privacy { get; set; }
        public string Category { get; set; }
        public PostVideoStatusEnum Status { get; set; }
        public string PostVideoCid { get; set; }
        public DateTime CreatedDate { get; set; }
        public List<Guid> Flags { get; set; }
        public Guid CategoryId { get; set; }

    }
}
