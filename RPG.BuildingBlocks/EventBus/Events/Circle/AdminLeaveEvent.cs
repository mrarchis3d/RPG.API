using System;

namespace RPG.BuildingBlocks.Common.EventBus.Events.Circle
{
    public class AdminLeaveCircleEvent
    {
        public Guid CircleId { get; set; }
        public Guid NewAdminId { get; set; }
    }
}