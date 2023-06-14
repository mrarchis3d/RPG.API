using Ourglass.Reactions.Domain.Enums;
using System;

namespace RPG.BuildingBlocks.Common.EventBus.Events.Audio
{
    public class AudioReactionEvent
    {
        public Guid Id { get; set; }
        public ReactionType ReactionType { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string ThumbnailCid { get; set; }
        public string AudioStreamUrl { get; set; }
        public string Cid { get; set; }
        public Guid UserId { get; set; }
        public Guid ReactedUserId { get; set; }
    }
}
