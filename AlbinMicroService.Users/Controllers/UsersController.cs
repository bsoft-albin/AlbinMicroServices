using System.ComponentModel.DataAnnotations;
using AlbinMicroService.Core.Controller;
using AlbinMicroService.Users.Application.Contracts;
using AlbinMicroService.Users.Domain;
using AlbinMicroService.Users.Domain.Models.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace AlbinMicroService.Users.Controllers
{
    [Route(ApiRoutes.API_TEMPLATE)]
    [ApiController]
    public class UsersController(IUsersAppContract _appContract, ILogger<UsersController> logger) : BaseController
    {
        [HttpPost]
        [ActionName(UsersActionNames.RegisterUser)]
        public async Task<IActionResult> CreateUserAsync([FromBody, Required] UserDto userDto)
        {
            return ParseApiResponse(await _appContract.CreateUserAppAsync(userDto), HttpVerbs.Post);
        }

        [HttpGet]
        [ActionName("get-custom-header")]
        public IActionResult AppendCustomHeader()
        {
            logger.LogInformation("Response Headers Sended");
            return GetResponseHeaders();
        }
    }
}
