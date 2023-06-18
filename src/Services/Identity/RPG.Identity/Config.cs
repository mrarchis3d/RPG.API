using IdentityServer4.Models;
using RPG.BuildingBlocks.Common.Constants;

namespace RPG.Identity
{
    public static class Config
    {
        public static IEnumerable<ApiResource> GetApiResources()=> new List<ApiResource>
        {
            new ApiResource("character")
            {
                Scopes = new List<string> { "character.full", SharedScopes.SCOPE_SERVICE_DISCOVERY},
                ApiSecrets = new List<Secret> {new Secret("c3e21893-0fe9-45ab-a885-27f9c158b2a1".Sha256())},
                UserClaims = new List<string> {"role"}
            }
        };
            

        public static IEnumerable<IdentityResource> GetIdentityResources() => new List<IdentityResource>
        {
            new IdentityResources.OpenId(),
            new IdentityResources.Profile(),
            new IdentityResource
            {
              Name = "role",
              UserClaims = new List<string> {"role"}
            }
        };
            

        public static IEnumerable<ApiScope> GetApiScopes() => new List<ApiScope>
        {
            new ApiScope(SharedScopes.SCOPE_SERVICE_DISCOVERY),
            new ApiScope("character.full", "full access to Character API")
        };
        
    }

}
