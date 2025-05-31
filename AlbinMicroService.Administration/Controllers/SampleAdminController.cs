using AlbinMicroService.Core.Controller;
using AlbinMicroService.Core.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AlbinMicroService.Administration.Controllers
{
    [Route(ApiRoutes.API_TEMPLATE)]
    [ApiController]
    [Authorize]
    public class SampleAdminController : BaseController
    {
        [HttpGet]
        [ActionName("sample-admin-endpoint")]
        public IActionResult Get()
        {
            return Ok("from Admin Service !!!");
        }
    }
}
