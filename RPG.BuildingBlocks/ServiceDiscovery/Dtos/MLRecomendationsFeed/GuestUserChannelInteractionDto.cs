using System;
namespace RPG.BuildingBlocks.Common.ServiceDiscovery.Dtos.MLRecomendationsFeed
{
    public class GuestUserChannelInteractionDto
    {
        public string GuestUserId { get; set; }
        public Guid ChannelId { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Event { get; set; }
    }
}
