using AlbinMicroService.Core.Controller;
using AlbinMicroService.Core.Utilities;
using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AlbinMicroService.Administration.Controllers
{
    [Route(ApiRoutes.API_VERSION_TEMPLATE)]
    [ApiVersion("1.0")]
    [ApiVersion("2.0")]
    [ApiController]
    [Authorize]
    public class SampleAdminController : BaseController
    {
        [HttpGet]
        [MapToApiVersion("1.0")]
        [ActionName("sample-admin-endpoint")]
        public IActionResult Get()
        {
            return Ok("from Admin Service !!!");
        }

        [HttpGet]
        [MapToApiVersion("2.0")]
        [ActionName("sample-admin-endpoint")]
        public IActionResult GetV2()
        {
            return Ok("from Admin Service !!!");
        }
    }
}
