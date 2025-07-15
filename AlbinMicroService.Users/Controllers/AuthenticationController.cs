using System.ComponentModel.DataAnnotations;
using AlbinMicroService.Core.Controller;
using AlbinMicroService.Libraries.BuildingBlocks.Authentication;
using AlbinMicroService.Libraries.Common.Entities;
using AlbinMicroService.Users.Application.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static AlbinMicroService.Core.Utilities.ApiAuthorization;

namespace AlbinMicroService.Users.Controllers
{
    [Route(ApiRoutes.API_TEMPLATE)]
    [ApiController]
    [AllowAnonymous]
    public class AuthenticationController(IHttpClientFactory clientFactory, IUsersAppContract usersAppContract, ITokenClient tokenClient, ILogger<AuthenticationController> logger, WebAppConfigs webAppConfigs) : BaseController
    {
        [HttpPost]
        [ActionName("login")]
        public async Task<IActionResult> Login([FromBody, Required] LoginRequestDto model)
        {
            if (string.IsNullOrWhiteSpace(model.Username) || string.IsNullOrWhiteSpace(model.Password))
            {
                return BadRequest("Invalid credentials");
            }

            string? userRole = await usersAppContract.GetUserRoleAppAsync(model.Username);

            if (string.IsNullOrWhiteSpace(userRole))
            {
                return Unauthorized("User not found");
            }

            using HttpClient client = clientFactory.CreateClient("IdentityServerHttpClient"); // creating a Named Http Client.

            string clientId = userRole switch
            {
                SystemRoles.SUPER_ADMIN => SystemClientIds.SUPER_ADMIN,
                SystemRoles.STAFF => SystemClientIds.STAFF,
                SystemRoles.MANAGER => SystemClientIds.MANAGER,
                SystemRoles.ADMIN => SystemClientIds.ADMIN,
                _ => SystemClientIds.USER
            };

            Dictionary<string, string> tokenRequest = new()
            {
                { TokenRequestKeys.grant_type, GrantTypes.password },
                { TokenRequestKeys.client_id, clientId },
                { TokenRequestKeys.username, model.Username },
                { TokenRequestKeys.password, model.Password }
            };

            if (userRole == SystemRoles.SUPER_ADMIN)
            {
                tokenRequest.Add(TokenRequestKeys.client_secret, SystemClientSecrets.SUPER_ADMIN);
            }
            if (userRole == SystemRoles.ADMIN)
            {
                tokenRequest.Add(TokenRequestKeys.client_secret, SystemClientSecrets.ADMIN);
            }

            HttpResponseMessage response = await client.PostAsync("connect/token", new FormUrlEncodedContent(tokenRequest));

            if (!response.IsSuccessStatusCode)
            {
                return Unauthorized(await response.Content.ReadAsStringAsync());
            }

            string content = await response.Content.ReadAsStringAsync();

            JsonElement data = JsonSerializer.Deserialize<JsonElement>(content);

            // Extract token details
            string accessToken = data.GetProperty("access_token").GetString()!;
            string refreshToken = data.GetProperty("refresh_token").GetString()!;
            string tokenType = data.GetProperty("token_type").GetString()!;
            string scopes = data.GetProperty("scope").GetString()!;
            int expiresIn = data.GetProperty("expires_in").GetInt32();
            DateTime expiresAt = StaticProps.AppDateUtcTimeNow.AddSeconds(expiresIn);

            Response.Cookies.Append("access_token", accessToken, new CookieOptions
            {
                HttpOnly = true,
                Secure = webAppConfigs.IsHavingSSL, // Only false in dev
                SameSite = SameSiteMode.Lax,
                Expires = expiresAt
            });

            Response.Cookies.Append("refresh_token", refreshToken, new CookieOptions
            {
                HttpOnly = true,
                Secure = webAppConfigs.IsHavingSSL, // Only false in dev
                SameSite = SameSiteMode.Lax,
                Expires = expiresAt.AddDays(7) //refresh token lives longer
            });

            return ParseApiResponse(new { TokenType = tokenType, Scopes = scopes }, HttpVerbs.Post);
        }

        [HttpPost]
        [ActionName("refresh-token")]
        public async Task<IActionResult> RefreshToken([FromBody, Required] RefreshTokenResult refreshTokenResult)
        {
            if (string.IsNullOrWhiteSpace(refreshTokenResult.RefreshToken))
            {
                return BadRequest("Refresh token is required.");
            }

            try
            {
                Dictionary<string, string> form = new()
                {
                    { TokenRequestKeys.grant_type, GrantTypes.refresh_token },
                    { TokenRequestKeys.client_id, SystemClientIds.ADMIN },
                    { TokenRequestKeys.client_secret, SystemClientSecrets.ADMIN },
                    { TokenRequestKeys.refresh_token, refreshTokenResult.RefreshToken }
                };

                TokenResponse tokenResponse = await tokenClient.RefreshTokenAsync(new RefreshTokenPayload { RefreshToken = refreshTokenResult.RefreshToken, RefreshPayload = form });

                Response.Cookies.Append("access_token", tokenResponse.AccessToken ?? string.Empty, new CookieOptions
                {
                    HttpOnly = true,
                    Secure = webAppConfigs.IsHavingSSL, // Only false in dev
                    SameSite = SameSiteMode.Lax,
                    Expires = tokenResponse.ExpiresAt
                });

                Response.Cookies.Append("refresh_token", tokenResponse.RefreshToken ?? string.Empty, new CookieOptions
                {
                    HttpOnly = true,
                    Secure = webAppConfigs.IsHavingSSL, // Only false in dev
                    SameSite = SameSiteMode.Lax,
                    Expires = tokenResponse.ExpiresAt.AddDays(7) //refresh token lives longer
                });

                return Ok(new
                {
                    access_token = tokenResponse.AccessToken,
                    refresh_token = tokenResponse.RefreshToken,
                    expires_at = tokenResponse.ExpiresAt
                });
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error Thrown");
                return Unauthorized(new { message = "Failed to get refresh token", error = ex.Message });
            }
        }
    }
}
