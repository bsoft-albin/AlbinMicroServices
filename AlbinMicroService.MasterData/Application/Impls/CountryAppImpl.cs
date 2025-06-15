using AlbinMicroService.Core;
using AlbinMicroService.MasterData.Application.Contracts;
using AlbinMicroService.MasterData.Domain.Models.Entities;
using AlbinMicroService.MasterData.Infrastructure.Contracts;

namespace AlbinMicroService.MasterData.Application.Impls
{
    public class CountryAppImpl(ICountryInfraContract countryInfraContract) : ICountryAppContract
    {
        public async Task<ApiObjectResponse> GetAllCountriesAppAsync()
        {
            ApiObjectResponse apiObjectResponse = new();
            List<Country> countries = await countryInfraContract.GetAllCountriesInfraAsync();
            if (countries.Count > 0)
            {
                apiObjectResponse.Data = countries;
                apiObjectResponse.StatusCode = HttpStatusCodes.Status200OK;
                apiObjectResponse.StatusMessage = HttpStatusMessages.Status200OK;
            }
            else
            {
                apiObjectResponse.StatusCode = HttpStatusCodes.Status204NoContent;
                apiObjectResponse.StatusMessage = HttpStatusMessages.Status204NoContent;
            }

            return apiObjectResponse;
        }
    }
}
