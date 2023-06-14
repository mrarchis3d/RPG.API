using System;
using System.Collections.Generic;

namespace RPG.BuildingBlocks.Common.EventBus.Events.Circle
{
    public class CircleUpdatedEvent
    {
        public Guid CircleId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<string> CircleTags { get; set; }
    }
}
