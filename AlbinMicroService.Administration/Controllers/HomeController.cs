using AlbinMicroService.Core.Controller;
using Microsoft.AspNetCore.Mvc;

namespace AlbinMicroService.Administration.Controllers
{
    [ApiController]
    public class HomeController : BaseController
    {
        [HttpGet]
        [Route("/")]
        public IActionResult Get()
        {
            return Ok("AlbinMicroServices Gateway Gets Started Running Successfully....");
        }

        [HttpGet]
        [Route("latest-version")]
        public IActionResult GetVersion()
        {
            return Ok("Version 1.0.0");
        }

        [HttpGet]
        [Route("/health")]
        public IActionResult GetHealth()
        {
            return Ok("Healthy");
        }
    }
}
