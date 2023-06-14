using System;

namespace RPG.BuildingBlocks.Common.EventBus.Events.PostAudio
{
    public class AudioTranscodedEvent
    {
        public Guid AudioId { get; set; }
    }
}