using System;
using System.Collections.Generic;
using RPG.BuildingBlocks.Common.Enums;

namespace RPG.BuildingBlocks.Common.ServiceDiscovery.Dtos.Wall;

public class WallByUserRequest
{
    public int Size { get; set; }
    public string Filter { get; set; }
    public TargetType? TargetType { get; set; }
    public List<Guid> FollowedChannels { get; set; } = new();
    public bool IsUserFriend { get; set; }
}