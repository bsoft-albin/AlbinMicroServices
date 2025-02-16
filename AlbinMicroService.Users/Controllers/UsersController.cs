using AlbinMicroService.Core.Controller;
using Microsoft.AspNetCore.Mvc;

namespace AlbinMicroService.Users.Controllers
{
    [Route(Templates.API_TEMPLATE)]
    [ApiController]
    public class UsersController : BaseController
    {
        [HttpGet]
        [ActionName("get-user-greet")]
        public IActionResult GetUserGreet()
        {
            return Ok("Hello User");
        }
    }
}
