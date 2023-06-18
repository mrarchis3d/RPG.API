using IdentityServer4.Models;
using IdentityServer4;
using RPG.BuildingBlocks.Common.Constants;

namespace RPG.Identity
{
    public static class ClientConfig
    {
        public static IEnumerable<Client> GetClients() => new List<Client>
        {
            new Client
            {
                ClientId = "unrealdesktop",
                ClientName = ClientIds.UnrealDesktop,
                ClientSecrets = {new Secret("SuperSecretPassword".Sha256())},
                AllowedGrantTypes ={ GrantType.ResourceOwnerPassword },
                RedirectUris = {"http://localhost:4000/"},
                AllowedCorsOrigins= {"http://localhost:4000"},
                RequireClientSecret = true,
                AllowAccessTokensViaBrowser = true,
                AllowedScopes =
                {
                    "character.full",
                    IdentityServerConstants.StandardScopes.OpenId,
                    IdentityServerConstants.StandardScopes.Profile,
                },
                AccessTokenLifetime = 3600, // Tiempo de vida del token en segundos
                RequireConsent = false,
                AllowOfflineAccess = true
            },
            new Client
            {
                ClientName = "Ourglass.Internal",
                ClientId = ClientIds.Internal,
                ClientSecrets = {new Secret("AnotherSuperSecretPassword".Sha256())},
                AllowedGrantTypes = GrantTypes.ClientCredentials,

                AllowedScopes = {
                SharedScopes.SCOPE_SERVICE_DISCOVERY
                }
            },
        };
    }
}
