using System;
using System.Collections.Generic;
using RPG.BuildingBlocks.Common.Enums;

namespace RPG.BuildingBlocks.Common.ServiceDiscovery.Dtos.Wall
{
    public class WallItemsRequest
    {
        public int Size { get; set; }
        public string Filter { get; set; }
        public TargetType? TargetType { get; set; }
        public Guid? TargetId { get; set; }
        public List<Guid> Friends { get; set; } = new List<Guid>();
        public List<Guid> FollowedChannels { get; set; } = new List<Guid>();
    }
}
