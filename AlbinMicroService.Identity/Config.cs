using Duende.IdentityServer.Models;
using Duende.IdentityServer.Test;

namespace AlbinMicroService.Identity
{
    public class Config
    {
        public static IEnumerable<IdentityResource> IdentityResources =>
        new IdentityResource[] {
            new IdentityResources.OpenId(),
            new IdentityResources.Profile()
        };

        public static IEnumerable<ApiScope> ApiScopes =>
            new ApiScope[] {
            new ApiScope("master.read", "Read Access to Master Api")
            };

        public static IEnumerable<Client> Clients =>
            new Client[] {
            new Client {
                ClientId = "albin-microservice-client",
                AllowedGrantTypes = GrantTypes.ClientCredentials,
                ClientSecrets = { new Secret("secret".Sha256()) },
                AllowedScopes = { "master.read" }
            }
            };

        public static List<TestUser> Users =>
            new List<TestUser> {
            new TestUser {
                SubjectId = "1",
                Username = "testuser",
                Password = "password"
            }
            };

        public static IEnumerable<ApiResource> ApiResources =>
            new[] {
            new ApiResource("masterapi", "master Api") {
                Scopes = { "master.read" }
            }
            };
    }
}
