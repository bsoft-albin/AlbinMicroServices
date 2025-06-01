using System.Security.Claims;
using Duende.IdentityServer.Models;
using Duende.IdentityServer.Test;

namespace AlbinMicroService.Identity
{
    public class IdentityServerConfigs
    {
        public static IEnumerable<IdentityResource> IdentityResources =>
        [
            new IdentityResources.OpenId(),
            new IdentityResources.Profile()
        ];

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

        // Here Client in the sense ==> web/app/desktop etc.. not user!!!
        // scopes and permissions are Same!!!
        public static IEnumerable<Client> Clients =>
        [
            //this is Especially for M2M Communication => Machine to Machine / Servive to Service
            // but ==> This does not [authenticate users]. It only authenticates apps.
            // For backend services (Client Credentials (M2M)) Method
            //new Client {
            //    ClientId = "backend-service-client",
            //    AllowedGrantTypes = GrantTypes.ClientCredentials, // client_credentials (while calling Token Endpoint)
            //    ClientSecrets = { new Secret("secret".Sha512()) }, // here we can use Different Words also, instead of [secret]
            //    AllowedScopes = { "master.read", "user.read" },
            //    AccessTokenLifetime = 60
            //},

            // [Currently we are using this]... later we can Upgrade it to below one (Authorization Code + PKCE)
            //Resource Owner Password Flow (ROPC) (not recommended for new apps, but useful for [Trusted apps and Legacy Apps])
            // here define each Client, for Each Roles/Devices/ app types [Mobile/Web/Desktop etc..]

            // 🔹 Regular Users — Mobile/Desktop/[Web/SPA] Clients
            new Client {
                ClientId = "public-client",
                AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                RequireClientSecret = false,
                AllowedScopes = { "openid", "profile", "user.read" },
                AllowOfflineAccess = true,
                AccessTokenLifetime = 60
            },
            // 🔸 Admin Client — Access to admin-only APIs
            new Client {
                ClientId = "admin-client",
                AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                RequireClientSecret = true, // more secure for admin
                ClientSecrets = { new Secret("admin-secret".Sha512()) },
                AllowedScopes = { "openid", "profile", "admin.read", "admin.write" },
                AllowOfflineAccess = true,
                AccessTokenLifetime = 60
            },
            // 🔺 Super Admin Client — Full access to all APIs
            new Client {
                ClientId = "superadmin-client",
                AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                RequireClientSecret = true, // more secure for admin
                ClientSecrets = { new Secret("superadmin-secret".Sha512()) },
                AllowedScopes = {
                    "openid", "profile",
                    "user.read", "master.read", "user.write","master.write",
                    "admin.read", "admin.write",
                    "system.full"
                },
                AllowOfflineAccess = true,
                AccessTokenLifetime = 60
            }

            // Later if you move to Authorization Code + PKCE, the frontend itself will get the token (via browser redirection). The backend (UserService) won't need to issue Token, manually anymore.

            // For SPA or WebApp or Mobile (Authorization Code + PKCE)
            //new Client {
            //    ClientId = "spa-client",
            //    AllowedGrantTypes = GrantTypes.Code,
            //    RequirePkce = true,
            //    RedirectUris = { "http://localhost:3000/callback" },
            //    AllowedScopes = { "openid", "profile", "master.read", "user.read" },
            //    AllowOfflineAccess = true,
            //    RequireClientSecret = false, // for private clients put ==> RequireClientSecret = true
            //    AccessTokenLifetime = 60
            //}
        ];

        // comment this when Db fetching... 
        //public static List<TestUser> Users =>
        //[
        //    new TestUser {
        //        SubjectId = "1",
        //        Username = "testuser",
        //        Password = "password",
        //        Claims =
        //        [
        //            new Claim("role", "admin"),
        //            new Claim("role", "user")
        //        ]
        //    },
        //    new TestUser {
        //        SubjectId = "2",
        //        Username = "manager1",
        //        Password = "password",
        //        Claims =
        //        [
        //            new Claim("role", "manager")
        //        ]
        //    }
        //];

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

    public class UserDto
    {
        public string Username { get; set; } = null!;
        //public string Name { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string Id { get; set; } = null!;
        public string Role { get; set; } = null!;
    }
}
