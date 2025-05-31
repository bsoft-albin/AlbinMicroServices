using AlbinMicroService.Core.Controller;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AlbinMicroService.MasterData.Controllers
{
    [Route(ApiRoutes.API_TEMPLATE)]
    [ApiController]
    [Authorize]
    public class CountryController : BaseController
    {
        [HttpGet]
        [ActionName("sample-country-endpoint")]
        public IActionResult Get()
        {
            return Ok("from Master Service !!!");
        }
    }
}
