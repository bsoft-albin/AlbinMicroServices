using AlbinMicroService.Core.Controller;
using AlbinMicroService.Core.Utilities;
using Microsoft.AspNetCore.Mvc;

namespace AlbinMicroService.Administration.Controllers
{
    [Route(Templates.API_TEMPLATE)]
    [ApiController]
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
