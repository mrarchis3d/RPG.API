using Ourglass.Reactions.Domain.Enums;
using System;

namespace RPG.BuildingBlocks.Common.EventBus.Events.PostVideo
{
    public class PostVideoReactionEvent
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }        
        public string PostVideoCid { get; set; }
        public string ThumbnailCid { get; set; }
        public DateTime CreatedDate { get; set; }
        public Guid ChannelId { get; set; }
        public string ChannelLongName { get; set; }
        public string videoStreamurl { get; set; }
        public Guid UserId { get; set; }
        public Guid ReactedUserId { get; set; }
        public ReactionType ReactionType { get; set; }

    }
}
