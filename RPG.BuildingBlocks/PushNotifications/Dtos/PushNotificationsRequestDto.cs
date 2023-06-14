namespace RPG.BuildingBlocks.Common.PushNotifications.Dtos
{
    public class PushNotificationsRequestDto
    {
        public string FcmToken { get; set; }
        public string DeviceId { get; set; }
        public string Language { get; set; }
    }
}