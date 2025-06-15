using AlbinMicroService.Core.Controller;
using AlbinMicroService.MasterData.Application.Contracts;
using AlbinMicroService.MasterData.Domain.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

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

        [HttpGet]
        [ActionName("get-country-byid")]
        public async Task<IActionResult> GetCountryById([FromQuery, Required] int countryId)
        {
            return ParseApiResponse(await countryAppContract.GetCountryByIdAppAsync(countryId), HttpVerbs.Get);
        }

        [HttpDelete]
        [ActionName("delete-country")]
        public async Task<IActionResult> DeleteCountry([FromQuery, Required] int countryId)
        {
            return ParseApiResponse(await countryAppContract.DeleteCountryByIdAppAsync(countryId), HttpVerbs.Delete);
        }

        [HttpPost]
        [ActionName("save-country")]
        public async Task<IActionResult> SaveCountry([FromBody, Required] Country country)
        {
            return ParseApiResponse(await countryAppContract.SaveCountryAppAsync(country), HttpVerbs.Post);
        }
    }
}
