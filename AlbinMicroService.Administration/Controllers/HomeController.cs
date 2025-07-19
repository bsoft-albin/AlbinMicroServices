using AlbinMicroService.Core.Controller;
using AlbinMicroService.Core.Utilities;
using AlbinMicroService.Kernel.Interfaces;
using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AlbinMicroService.Administration.Controllers
{
    [ApiController]
    [Route(ApiRoutes.API_VERSION_TEMPLATE)]
    [ApiVersion("1.0")]
    [ApiVersion("2.0")]
    [AllowAnonymous]
    public class HomeController(IKernelMeths _kernelMeths) : BaseController
    {
        [HttpGet]
        [Route("/")]
        [MapToApiVersion("1.0")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public IActionResult Get()
        {
            return Content(_kernelMeths.GetTextFileContents("Welcome.html"), contentType: "text/html");
        }

        [HttpGet]
        [MapToApiVersion("1.0")]
        [ActionName("latest-version")]
        public IActionResult GetVersion()
        {
            return Ok("Version 1.0.0");
        }
        
        [HttpGet]
        [MapToApiVersion("1.0")]
        [ActionName("is-running")]
        public IActionResult RunsOrNot()
        {
            return Ok("AlbinMicroServices Administration Gets Started Running Successfully....");
        }

        [HttpGet]
        [MapToApiVersion("1.0")]
        [ActionName("health")]
        public IActionResult GetHealth()
        {
            return Ok("Healthy");
        }
    }
}
