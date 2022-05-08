using IdentityServer4.Models;

namespace Identity.API.Configuration
{
    public class Config
    {
        public static IEnumerable<IdentityResource> IdentityResources =>
            new[]
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResource
                {
                      Name = "role",
                      UserClaims = new List<string>{"role"}
                }
            };

        public static IEnumerable<ApiScope> ApiScopes =>
            new[] {
                new ApiScope("Product.API.Read"),
                new ApiScope("Product.API.Write")
            };

        public static IEnumerable<ApiResource> ApiResources =>
            new[]
            {
                new ApiResource("Product.API")
                {
                    Scopes = new List<string>{ "Product.API.Read", "Product.API.Write" },
                    ApiSecrets = new  List<Secret> { new Secret("ScopeSecert".Sha256()) },
                    UserClaims =new List<string>{ "role"}
                }
            };

        public static IEnumerable<Client> Clients =>
           new[]
           {
              new  Client
              {
                  ClientId = "m2m.client",
                  ClientName ="Client Credentials Client",
                  AllowedGrantTypes = GrantTypes.ClientCredentials,
                  ClientSecrets = {new Secret("ClientSecrets1".Sha256())},
                  AllowedScopes = { "Product.API.Read", "Product.API.Write" }
              },
              new Client
              {
                  ClientId = "interactive",
                  ClientSecrets = { new Secret("ClientSecrets1".Sha256()) },
                  AllowedGrantTypes = GrantTypes.Code,
                  RedirectUris=  { "https://localhost:7514/signin-oidc" },
                  FrontChannelLogoutUri = "https://localhost:7414/singout-oidc",
                  PostLogoutRedirectUris= { "https://localhost:7414/signout-callback-oidc" },
                  AllowOfflineAccess = true,
                  AllowedScopes = {"openid","profile", "Product.API.Read" },
                  RequirePkce = true,
                  RequireConsent = true,
                  AllowPlainTextPkce = false
              }
           };
    }
}
