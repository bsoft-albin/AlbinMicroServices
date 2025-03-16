using AlbinMicroService.Core.Controller;
using AlbinMicroService.Users.Application.Contracts;
using AlbinMicroService.Users.Domain;
using AlbinMicroService.Users.Domain.DTOs;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace AlbinMicroService.Users.Controllers
{
    [Route(Templates.API_TEMPLATE)]
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
        [ActionName("data-check")]
        public IActionResult HealthCheck()
        {
            logger.LogInformation("Health check endpoint hit");
            return GetResponseHeaders();
        }
    }
}
