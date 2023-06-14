using RPG.BuildingBlocks.Common.Enums;
using System;

namespace RPG.BuildingBlocks.Common.ServiceDiscovery.Dtos.Wall
{
    public class CommentDto
    {
        public string CommentId { get; set; }
        public string Text { get; set; }
        public string ContentId { get; set; }
        public ContentType ContentType { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
        public UserDto User { get; set; }
    }

    public class UserDto
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string ThumbnailCid { get; set; }
    }
}
