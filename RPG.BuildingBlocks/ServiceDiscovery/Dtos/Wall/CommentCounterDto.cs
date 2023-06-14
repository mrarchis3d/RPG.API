using RPG.BuildingBlocks.Common.Enums;

namespace RPG.BuildingBlocks.Common.ServiceDiscovery.Dtos.Wall
{
    public class CommentCounterDto
    {
        public string ContentId { get; set; }
        public ContentType ContentType { get; set; }
        public string Counter { get; set; }
    }
}
