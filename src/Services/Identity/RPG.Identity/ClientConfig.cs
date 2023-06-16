using IdentityServer4.Models;
using IdentityServer4;

namespace RPG.Identity
{
    public static class ClientConfig
    {
        public static IEnumerable<Client> GetClients() => new List<Client>
        {
            new Client
            {
                ClientId = "unrealdesktop",
                ClientName = "unrealdesktop",
                AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                ClientSecrets =
                {
                    new Secret("clientSecret".Sha256())
                },
                AllowedScopes =
                {
                    "character.read",
                    "character.write",
                    IdentityServerConstants.StandardScopes.OpenId,
                    IdentityServerConstants.StandardScopes.Profile,
                },
                AccessTokenLifetime = 3600, // Tiempo de vida del token en segundos
            }
        };
    }
}
