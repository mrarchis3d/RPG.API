using System;
using System.Collections.Generic;

namespace RPG.BuildingBlocks.Common.EventBus.Events.Channel
{
    public class ChannelCreatedEvent
    {
        public Guid ChannelId { get; set; }
        public Guid UserId { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string ChannelShortName { get; set; }
        public string ChannelLongName { get; set; }
        public string ChannelDescription { get; set; }
        public List<string> ChannelTags { get; set; }
        public List<string> ChannelAudienceLocations { get; set; }
        public int ChannelAudienceMinAge { get; set; }
        public int ChannelAudienceMaxAge { get; set; }
    }
}
