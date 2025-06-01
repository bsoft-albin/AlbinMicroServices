using System.ComponentModel.DataAnnotations;
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
    public class UsersController(IUsersAppContract _appContract, ILogger<UsersController> logger) : BaseController
    {
        [HttpPost]
        //[MapToApiVersion("1.0")]
        [ActionName(UsersActionNames.RegisterUser)]
        public async Task<IActionResult> CreateUserAsync([FromBody, Required] UserDto userDto)
        {
            return ParseApiResponse(await _appContract.CreateUserAppAsync(userDto), HttpVerbs.Post);
        }

        //[HttpPost]
        //[ActionName("login")]
        //public async Task<IActionResult> Login([FromBody, Required] LoginRequestDto model) // LoginRequest Built-in Model for Login, we can use Ours !...
        //{
        //    // Optional: validate from your own DB if you want additional logic
        //    if (string.IsNullOrWhiteSpace(model.Username) || string.IsNullOrWhiteSpace(model.Password))
        //    {
        //        return BadRequest("Invalid credentials");
        //    }

        //    HttpClient client = new();
        //    var disco = await client.GetDiscoveryDocumentAsync("http://localhost:9998");
        //    if (disco.IsError)
        //        return StatusCode(500, disco.Error);

        //    var tokenResponse = await client.RequestPasswordTokenAsync(new PasswordTokenRequest
        //    {
        //        Address = disco.TokenEndpoint,
        //        ClientId = "mobile-desktop-spa-and-webapp-client",
        //        //ClientSecret = "optional", // only if client secret required
        //        UserName = model.Username,
        //        Password = model.Password,
        //        Scope = "openid profile api1.read"
        //    });

        //    if (tokenResponse.IsError)
        //        return Unauthorized(tokenResponse.Error);

        //    return Ok(new
        //    {
        //        access_token = tokenResponse.AccessToken,
        //        expires_in = tokenResponse.ExpiresIn,
        //        refresh_token = tokenResponse.RefreshToken,
        //        token_type = tokenResponse.TokenType,
        //        scope = tokenResponse.Scope
        //    });
        //}

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
