using System;
using System.Collections.Generic;

namespace RPG.BuildingBlocks.Common.ServiceDiscovery.Dtos.PushNotifications
{
    public class GetDevicesDto
    {
        public Guid UserId { get; set; }
        public List<DeviceResponse> Devices { get; set; }
        public List<AppResponse> Apps { get; set; }
    }

    public class DeviceResponse
    {
        public string DeviceId { get; set; }
        public string FcmToken { get; set; }
        public string Language { get; set; }
    }

    public class AppResponse
    {
        public string AppName { get; set; }
    }
}
