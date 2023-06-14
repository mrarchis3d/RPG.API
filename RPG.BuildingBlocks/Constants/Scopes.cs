using System.Collections.Generic;

namespace RPG.BuildingBlocks.Common.Constants
{
    public static class Scopes
    {
        public static Dictionary<string, string> ApiScopes =>
            new Dictionary<string, string>()
            {
                {"openid", "openid"},
                {"profile", "profile"},
                {"offline_access", "offline access"},
                {"follow.full", "follow full access"},
                {"videomanager.full", "videomanager full access"},
                {"liveeventmanager.full", "liveeventmanager full access"},
                {"liveconcert.full", "liveconcert full access"},
                {"imagesmanager.full", "imagesmanager full access"},
                {"channel.full", "channel full access"},
                {"circle.full", "circle full access"},
                {"friends.full", "friends full access"},
                {"search.full", "search full access"},
                {"scheduling.full", "scheduling full access"},
                {"tier.full", "tier full access"},
                {"userprofiledataaggregate.full", "userprofiledataaggregate full access"},
                {"postvideo.full", "postvideo full access"},
                {"mlrecomendationsfeed.full", "mlrecomendationsfeed full access"},
                {"chat.full", "chat full access"},
                {"filemanager.full", "filemanager full access"},
                {"notificationmanager.full", "notificationmanager full access"},
                {"languagemanager.full", "languagemanager full access"},
                {"comments.full", "comments full access"},
                {"identity.full", "identity full access"},
                {"statistics.full", "statistics full access"},
                {"reactions.full", "reactions full access"},
                {"audiomanager.full", "audiomanager full access"},
                {"textpost.full", "textpost full access"},
                {"invitecodes.full", "invitecodes full access"},
                {"wallaggregate.full", "wallaggregate full access"},
                {"contentindex.full", "contentindex full access"},
                {"userinfo.full", "userinfo full access"},
                {"realtime.full", "realtime full access"},
            };
    }
}