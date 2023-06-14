using System;
namespace RPG.BuildingBlocks.Common.ServiceDiscovery.Dtos.MLRecomendationsFeed
{
    public class LoggedUserChannelInteractionDto
    {
        public Guid UserId { get; set; }
        public Guid ChannelId { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Event { get; set; }
    }
}
