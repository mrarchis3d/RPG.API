using IdentityServer4.Models;

namespace RPG.Identity
{
    public static class Config
    {
        public static IEnumerable<ApiResource> GetApiResources()=> new List<ApiResource>
        {
            new ApiResource("character", "Character")
        };
            

        public static IEnumerable<IdentityResource> GetIdentityResources() => new List<IdentityResource>
        {
            new IdentityResources.OpenId(),
            new IdentityResources.Profile(),
        };
            

        public static IEnumerable<ApiScope> GetApiScopes() => new List<ApiScope>
        {
            new ApiScope("character.read", "Read access to Character API"),
            new ApiScope("character.write", "Write access to Character API"),
        };
        
    }

}
