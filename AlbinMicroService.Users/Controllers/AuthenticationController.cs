using AlbinMicroService.Core.Controller;
using AlbinMicroService.Users.Application.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AlbinMicroService.Users.Controllers
{
    [Route(ApiRoutes.API_TEMPLATE)]
    [ApiController]
    [AllowAnonymous]
    public class AuthenticationController(IHttpClientFactory clientFactory, IUsersAppContract usersAppContract) : BaseController
    {
        [HttpPost]
        [ActionName("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto model)
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
                { "grant_type", "password" },
                { "client_id", clientId },
                { "username", model.Username },
                { "password", model.Password }
            };

            if (userRole == SystemRoles.SUPER_ADMIN)
            {
                tokenRequest.Add("client_secret", SystemClientSecrets.SUPER_ADMIN);
            }
            if (userRole == SystemRoles.ADMIN)
            {
                tokenRequest.Add("client_secret", SystemClientSecrets.ADMIN);
            }

            HttpResponseMessage response = await client.PostAsync("connect/token", new FormUrlEncodedContent(tokenRequest));

            if (!response.IsSuccessStatusCode)
            {
                return Unauthorized(await response.Content.ReadAsStringAsync());
            }

            string content = await response.Content.ReadAsStringAsync();

            JsonElement data = JsonSerializer.Deserialize<JsonElement>(content);

            return ParseApiResponse(data, HttpVerbs.Post);
        }
    }
}
