using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using AlbinMicroService.Core.Controller;
using AlbinMicroService.Users.Application.Contracts;
using AlbinMicroService.Users.Domain;
using AlbinMicroService.Users.Domain.Models.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AlbinMicroService.Users.Controllers
{
    //[Route(ApiRoutes.API_VERSION_TEMPLATE)]
    //[ApiVersion("1.0")]
    //[ApiVersion("2.0")]
    [Route(ApiRoutes.API_TEMPLATE)]
    [ApiController]
    [AllowAnonymous]
    public class UsersController(IUsersAppContract _appContract, ILogger<UsersController> logger) : BaseController
    {
        [HttpPost]
        //[MapToApiVersion("1.0")]
        [ActionName(UsersActionNames.RegisterUser)]
        public async Task<IActionResult> CreateUserAsync([FromBody, Required] UserDto userDto)
        {
            return ParseApiResponse(await _appContract.CreateUserAppAsync(userDto), HttpVerbs.Post);
        }

        [HttpPost]
        [ActionName("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto model)
        {
            if (string.IsNullOrWhiteSpace(model.Username) || string.IsNullOrWhiteSpace(model.Password))
            {
                return BadRequest("Invalid credentials");
            }

            using HttpClient client = new();

            string userRole = ""; // here need to make a db call to fecth userrole only with username....

            if (string.IsNullOrWhiteSpace(userRole))
            {
                return Unauthorized("User not found");
            }

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

            HttpResponseMessage response = await client.PostAsync("http://localhost:9998/connect/token", new FormUrlEncodedContent(tokenRequest));

            if (!response.IsSuccessStatusCode)
            {
                return Unauthorized(await response.Content.ReadAsStringAsync());
            }

            string content = await response.Content.ReadAsStringAsync();

            JsonElement data = JsonSerializer.Deserialize<JsonElement>(content);

            return ParseApiResponse(data, HttpVerbs.Post); // or deserialize into a DTO
        }

        [Authorize(Policy = "AdminOnly")]
        [HttpGet]
        //[MapToApiVersion("1.0")]
        [ActionName("get-custom-header")]
        public IActionResult AppendCustomHeader()
        {
            logger.LogInformation("Response Headers Sended");
            return GetResponseHeaders();
        }

        [Authorize(Policy = ApiAuthorization.Policies.AdminOnly)]
        [HttpGet]
        [ActionName("get-v1")]
        //[MapToApiVersion("1.0")]
        public IActionResult GetV1() => Ok("Users V1");

        [Authorize(Policy = "UserReadScope")]
        [HttpGet]
        [ActionName("get-v2")]
        //[MapToApiVersion("1.0")]
        public IActionResult GetV2() => Ok("Users V2");
    }
}
