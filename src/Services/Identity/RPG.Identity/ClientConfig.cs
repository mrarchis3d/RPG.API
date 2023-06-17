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
                ClientName = "unrealdesktop client",
                AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                ClientSecrets =
                {
                    new Secret("secret".Sha256())
                },
                AllowedScopes =
                {
                    "character.read",
                    "character.write",
                    IdentityServerConstants.StandardScopes.OpenId,
                    IdentityServerConstants.StandardScopes.Profile,
                },
                AccessTokenLifetime = 3600, // Tiempo de vida del token en segundos

                RequireConsent = false,
                RequirePkce = true,

                // where to redirect to after login
                RedirectUris = { "http://localhost:5002/signin-oidc" },

                // where to redirect to after logout
                PostLogoutRedirectUris = { "http://localhost:5002/signout-callback-oidc" },


                AllowOfflineAccess = true
            }
        };
    }
}
