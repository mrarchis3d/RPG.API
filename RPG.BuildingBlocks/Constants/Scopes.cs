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
                {"character.full", "character access"},
            };
    }
}