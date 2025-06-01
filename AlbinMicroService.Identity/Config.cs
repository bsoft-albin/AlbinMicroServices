using System.Net;
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
            //this is Especially for M2M Communication => Machine to Machine / Servive to Service
            // but ==> This does not [authenticate users]. It only authenticates apps.
            // For backend services (Client Credentials (M2M)) Method
            new Client {
                ClientId = "backend-service-client",
                AllowedGrantTypes = GrantTypes.ClientCredentials, // client_credentials (while calling Token Endpoint)
                ClientSecrets = { new Secret("secret".Sha512()) },
                AllowedScopes = { "master.read", "user.read" },
                AccessTokenLifetime = 60
            },

            // [Currently we are using this]... later we can Upgrade it to below one (Authorization Code + PKCE)
            //Resource Owner Password Flow (ROPC) (not recommended for new apps, but useful for [Trusted apps and Legacy Apps])
            new Client {
                ClientId = "mobile-desktop-spa-and-webapp-client",
                AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                RequireClientSecret = false, // for mobile / SPA / WebApp / Desktop etc...
                AllowedScopes = { "openid", "profile", "api1.read" },
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

        public static List<TestUser> Users =>
        [
            new TestUser {
                SubjectId = "1",
                Username = "testuser",
                Password = "password",
                Claims =
                [
                    new Claim("role", "admin"),
                    new Claim("role", "user")
                ]
            },
            new TestUser {
                SubjectId = "2",
                Username = "manager1",
                Password = "password",
                Claims =
                [
                    new Claim("role", "manager")
                ]
            }
        ];

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
