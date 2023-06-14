using System;
namespace RPG.BuildingBlocks.Common.EventBus.Events.User
{
    public class UserCreatedEvent
    {
        public Guid UserId { get; set; }
        public string GuestUserId { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
