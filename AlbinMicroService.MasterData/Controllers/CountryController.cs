using AlbinMicroService.Core.Controller;
using AlbinMicroService.MasterData.Application.Contracts;
using AlbinMicroService.MasterData.Domain.Models.Entities;
using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace AlbinMicroService.MasterData.Controllers
{
    [Route(ApiRoutes.API_VERSION_TEMPLATE)]
    [ApiVersion("1.0")]
    [ApiVersion("2.0")]
    [ApiController]
    [AllowAnonymous]
    public class CountryController(ICountryAppContract countryAppContract) : BaseController
    {
        [HttpGet]
        [MapToApiVersion("1.0")]
        [ActionName("get-all-counties")]
        public async Task<IActionResult> GetAllCountries()
        {
            return ParseApiResponse(await countryAppContract.GetAllCountriesAppAsync(), HttpVerbs.Get);
        }

        [HttpGet]
        [MapToApiVersion("1.0")]
        [ActionName("get-country-byid")]
        public async Task<IActionResult> GetCountryById([FromQuery, Required] int countryId)
        {
            return ParseApiResponse(await countryAppContract.GetCountryByIdAppAsync(countryId), HttpVerbs.Get);
        }

        [HttpGet]
        [MapToApiVersion("2.0")]
        [ActionName("get-country-byid")]
        public async Task<IActionResult> GetCountryByIdV2([FromQuery, Required] int countryId)
        {
            return ParseApiResponse(await countryAppContract.GetCountryByIdAppAsync(countryId), HttpVerbs.Get);
        }

        [HttpDelete]
        [MapToApiVersion("1.0")]
        [ActionName("delete-country")]
        public async Task<IActionResult> DeleteCountry([FromQuery, Required] int countryId)
        {
            return ParseApiResponse(await countryAppContract.DeleteCountryByIdAppAsync(countryId), HttpVerbs.Delete);
        }

        [HttpPost]
        [MapToApiVersion("1.0")]
        [ActionName("save-country")]
        public async Task<IActionResult> SaveCountry([FromBody, Required] Country country)
        {
            return ParseApiResponse(await countryAppContract.SaveCountryAppAsync(country), HttpVerbs.Post);
        }
    }
}
