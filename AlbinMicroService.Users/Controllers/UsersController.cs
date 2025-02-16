using AlbinMicroService.Core.Controller;
using AlbinMicroService.Users.Application.Contracts;
using AlbinMicroService.Users.Domain.DTOs;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace AlbinMicroService.Users.Controllers
{
    [Route(Templates.API_TEMPLATE)]
    [ApiController]
    public class UsersController(IUsersAppContract _appContract) : BaseController
    {
        [HttpPost]
        [ActionName("register-user")]
        public async Task<IActionResult> CreateUserAsync([FromBody, Required] UserDto userDto)
        {
            return Ok(await _appContract.CreateUserAsync(userDto));
        }
    }
}
