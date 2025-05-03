using AlbinMicroService.Core.Controller;
using AlbinMicroService.Core.Utilities;
using AlbinMicroService.Kernel.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AlbinMicroService.Administration.Controllers
{
    [ApiController]
    [Route(Templates.API_TEMPLATE)]
    public class HomeController(IKernelMeths _kernelMeths) : BaseController
    {
        [HttpGet]
        [Route("/")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public IActionResult Get()
        {
            return Content(_kernelMeths.GetTextFileContents("Welcome.html"), contentType: "text/html");
        }

        [HttpGet]
        [ActionName("latest-version")]
        public IActionResult GetVersion()
        {
            return Ok("Version 1.0.0");
        }
        
        [HttpGet]
        [ActionName("is-running")]
        public IActionResult RunsOrNot()
        {
            return Ok("AlbinMicroServices Administration Gets Started Running Successfully....");
        }

        [HttpGet]
        [ActionName("health")]
        public IActionResult GetHealth()
        {
            return Ok("Healthy");
        }
    }
}
