using AlbinMicroService.Core.Controller;
using AlbinMicroService.MasterData.Application.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AlbinMicroService.MasterData.Controllers
{
    [Route(ApiRoutes.API_TEMPLATE)]
    [ApiController]
    [Authorize]
    [AllowAnonymous]
    public class CountryController(ICountryAppContract countryAppContract) : BaseController
    {
        [HttpGet]
        [ActionName("get-all-counties")]
        public async Task<IActionResult> GetAllCountries()
        {
            return ParseApiResponse(await countryAppContract.GetAllCountriesAppAsync(), HttpVerbs.Get);
        }
    }
}
