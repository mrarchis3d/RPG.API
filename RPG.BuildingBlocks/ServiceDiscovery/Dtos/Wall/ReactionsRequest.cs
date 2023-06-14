using RPG.BuildingBlocks.Common.Enums;
using System;

namespace RPG.BuildingBlocks.Common.ServiceDiscovery.Dtos.Wall
{
    public class ReactionsRequest
    {
        public Guid ContentId { get; set; }
        public ReactionContentType ReactionContentType { get; set; }
    }
}
