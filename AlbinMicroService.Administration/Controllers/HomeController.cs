using AlbinMicroService.Core.Controller;
using AlbinMicroService.Kernel.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AlbinMicroService.Administration.Controllers
{
    [ApiController]
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
        [Route("latest-version")]
        public IActionResult GetVersion()
        {
            return Ok("Version 1.0.0");
        }
        
        [HttpGet]
        [Route("is-running")]
        public IActionResult RunsOrNot()
        {
            return Ok("AlbinMicroServices Gateway Gets Started Running Successfully....");
        }

        [HttpGet]
        [Route("/health")]
        public IActionResult GetHealth()
        {
            return Ok("Healthy");
        }
    }
}
