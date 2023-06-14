using RPG.BuildingBlocks.Common.Enums;
using System;
using System.Collections.Generic;

namespace RPG.BuildingBlocks.Common.ServiceDiscovery.Dtos.Wall
{
    public class ReactionsDto
    {
        public ContentType ContentType { get; set; }
        public Guid ContentId { get; set; }
        public List<Counter> Counters { get; set; }
        public int? UserReactionType { get; set; }
    }

    public class Counter
    {
        public int Type { get; set; }
        public int Value { get; set; }
    }
}
