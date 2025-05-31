using System.Security.Claims;
using Duende.IdentityServer.Models;
using Duende.IdentityServer.Test;

namespace AlbinMicroService.Identity
{
    public class Config
    {
        public static IEnumerable<IdentityResource> IdentityResources =>
        [
            new IdentityResources.OpenId(),
            new IdentityResources.Profile()
        ];

        //public static IEnumerable<ApiScope> ApiScopes =>
        //    [
        //    new ApiScope("master.read", "Read Access to Master Api")
        //    ];

        public static IEnumerable<ApiScope> ApiScopes =>
                [
                    new ApiScope("master.read"),
                    new ApiScope("master.write"),
                    new ApiScope("user.read"),
                    new ApiScope("user.write"),
                    new ApiScope("admin.read"),
                    new ApiScope("admin.write"),
                    new ApiScope("offline_access") // For refresh tokens
                ];

        public static IEnumerable<Client> Clients =>
            [
            new Client {
                ClientId = "albin-microservice-client",
                AllowedGrantTypes = GrantTypes.ClientCredentials,
                ClientSecrets = { new Secret("secret".Sha256()) },
                AllowedScopes = { "master.read", "user.read" }, // ✅ Add this if want need to add more
                AccessTokenLifetime = 60
            }
            ];

        //public static List<TestUser> Users =>
        //    [
        //    new TestUser {
        //        SubjectId = "1",
        //        Username = "testuser",
        //        Password = "password"
        //    }
        //    ];

        public static List<TestUser> Users =>
[
    new TestUser {
        SubjectId = "1",
        Username = "testuser",
        Password = "password",
        Claims = new List<Claim>
        {
            new Claim("role", "admin"),
            new Claim("role", "user")
        }
    },
    new TestUser {
        SubjectId = "2",
        Username = "manager1",
        Password = "password",
        Claims = new List<Claim>
        {
            new Claim("role", "manager")
        }
    }
];

        //public static IEnumerable<ApiResource> ApiResources =>
        //    [
        //    new ApiResource("masterapi", "master Api") {
        //        Scopes = { "master.read" }
        //    }
        //    ];

        public static IEnumerable<ApiResource> ApiResources =>
[
    new ApiResource("masterapi", "Master API")
    {
        Scopes = { "master.read", "master.write" }
    },
    new ApiResource("userapi", "User API")
    {
        Scopes = { "user.read", "user.write" }
    },
    new ApiResource("adminapi", "Admin API")
    {
        Scopes = { "admin.read", "admin.write" }
    }
];
    }
}
