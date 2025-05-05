using AlbinMicroService.Core.Controller;
using Microsoft.AspNetCore.Mvc;

namespace AlbinMicroService.MasterData.Controllers
{
    [Route(ApiRoutes.API_TEMPLATE)]
    [ApiController]
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
