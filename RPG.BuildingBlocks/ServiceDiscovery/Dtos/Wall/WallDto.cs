using RPG.BuildingBlocks.Common.Enums;
using System;
using System.Collections.Generic;

namespace RPG.BuildingBlocks.Common.ServiceDiscovery.Dtos.Wall
{
    public class WallDto
    {
        public List<WallItem> Items { get; set; }
        public string PrevCursor { get; set; }
        public string NextCursor { get; set; }
    }

    public class WallItem
    {
        public string ContentId { get; set; }
        public ContentType Type { get; set; }
        public string UserId { get; set; }
        public string UserThumbnailCid { get; set; }
        public TargetType TargetType { get; set; }
        public string TargetId { get; set; }
        public string ChannelShortName { get; set; }
        public string ChannelLongName { get; set; }
        public string ChannelThumbnailCid { get; set; }
        public string Fullname { get; set; }
        public string Username { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string ThumbnailCid { get; set; }
        public string Cid { get; set; }
        public DateTime CreatedDate { get; set; }
        public ReactionsDto Reactions { get; set; }
        public string CommentsCounter { get; set; }
        public Privacy Privacy { get; set; }
        public string StreamUrl { get; set; }
        public long Views { get; set; }
        public object ExtraData { get; set; }
    }

    public class ExtraData
    {
        public string Type { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
    }
}
