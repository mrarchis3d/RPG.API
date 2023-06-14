using System;
namespace RPG.BuildingBlocks.Common.EventBus.Events.User
{
    public class UserUpdatedThumbnailEvent
    {
        public Guid UserId { get; set; }
        public string Cid { get; set; }
    }
}
