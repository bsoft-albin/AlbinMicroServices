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

        [Authorize(Policy = "AdminOnly")]
        [HttpGet]
        //[MapToApiVersion("1.0")]
        [ActionName("get-custom-header")]
        public IActionResult AppendCustomHeader()
        {
            logger.LogInformation("Response Headers Sended");
            return GetResponseHeaders();
        }

        [Authorize(Policy = "AdminOnly")]
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
