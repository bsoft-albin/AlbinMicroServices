using System.ComponentModel.DataAnnotations;
using AlbinMicroService.Core;
using AlbinMicroService.Core.Controller;
using AlbinMicroService.Users.Application.Contracts;
using AlbinMicroService.Users.Domain;
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
    public class UsersController(IUsersAppContract appContract, ILogger<UsersController> logger) : BaseController
    {
        [HttpPost]
        //[MapToApiVersion("1.0")]
        [ActionName(UsersActionNames.RegisterUser)]
        [ProducesResponseType(typeof(ApiBaseResponse), HttpStatusCodes.Status201Created)]
        [ProducesResponseType(HttpStatusCodes.Status400BadRequest)]
        [ProducesResponseType(HttpStatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateUserAsync([FromBody, Required] UserRegisterDto userDto)
        {
            return ParseApiResponse(await appContract.CreateUserAppAsync(userDto), HttpVerbs.Post);
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
