using Duende.IdentityServer;
using Duende.IdentityServer.Models;
using RPG.BuildingBlocks.Common.Constants;

namespace RPG.Identity
{
    public class Config
    {
        public static IEnumerable<ApiResource> ApiResources => new[]
        {

              new ApiResource("character")
              {
                Scopes = new List<string> {"character.full", SharedScopes.SCOPE_SERVICE_DISCOVERY},
                ApiSecrets = new List<Secret> {new Secret("348272ee-2dc1-4b84-bf94-41d7242ab622".Sha256())},
                UserClaims = new List<string> {"role"}
              }
        };

        public static IEnumerable<IdentityResource> IdentityResources =>
          new[]
          {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResource
                {
                  Name = "role",
                  UserClaims = new List<string> {"role"}
                }
          };

        //add further scopes to Scopes in BuildingBlocks (do not add Pubsub) 
        public static IEnumerable<ApiScope> ApiScopes =>
          new[]
          {
                //client credential client
                new ApiScope(SharedScopes.SCOPE_SERVICE_DISCOVERY),
                //flutter client
                new ApiScope("character.full")
          };

        public static IEnumerable<Client> Clients =>
          new[]
          {        
            //Client for resource owner password grant (mobile)
            new Client
            {
              ClientName = "RPG.UnrealMobile",
              ClientId = ClientIds.UnrealMobile,
              ClientSecrets = {new Secret("SuperSecretPassword".Sha256())},
              AllowedGrantTypes = { "authorization_code", "password"},
              RequirePkce = true,
              RequireClientSecret = false,

              RedirectUris = {"http://localhost:4000/"},
              AllowedCorsOrigins= {"http://localhost:4000"},

              AllowedScopes = {
                IdentityServerConstants.StandardScopes.OpenId,
                IdentityServerConstants.StandardScopes.Profile,
                IdentityServerConstants.StandardScopes.OfflineAccess,
                "character.full"
              },
              AllowAccessTokensViaBrowser = true,
              RequireConsent = false,
              AllowOfflineAccess = true,
              AccessTokenLifetime = 3600
            },
            new Client
            {
              ClientName = "RPG.Internal",
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
