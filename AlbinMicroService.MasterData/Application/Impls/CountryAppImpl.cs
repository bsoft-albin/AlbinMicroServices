using AlbinMicroService.Core;
using AlbinMicroService.MasterData.Application.Contracts;
using AlbinMicroService.MasterData.Domain.Models.Dtos;
using AlbinMicroService.MasterData.Domain.Models.Entities;
using AlbinMicroService.MasterData.Infrastructure.Contracts;

namespace AlbinMicroService.MasterData.Application.Impls
{
    public class CountryAppImpl(ICountryInfraContract countryInfraContract) : ICountryAppContract
    {
        public async Task<ApiBaseResponse> DeleteCountryByIdAppAsync(int countryId)
        {
            ApiBaseResponse apiBaseResponse = new();
            bool? result = await countryInfraContract.DeleteCountryByIdInfraAsync(countryId);
            if (result != null && result == true)
            {
                apiBaseResponse.StatusCode = HttpStatusCodes.Status200OK;
                apiBaseResponse.StatusMessage = HttpStatusMessages.Status200OK;
            }
            else if (result == null)
            {
                apiBaseResponse.StatusCode = HttpStatusCodes.Status204NoContent;
                apiBaseResponse.StatusMessage = HttpStatusMessages.Status204NoContent;
            }
            else
            {
                apiBaseResponse.StatusCode = CustomHttpStatusCodes.UnXpectedError;
                apiBaseResponse.StatusMessage = CustomHttpStatusMessages.UnXpectedError;
            }

            return apiBaseResponse;
        }

        public async Task<ApiObjectResponse> GetAllCountriesAppAsync()
        {
            ApiObjectResponse apiObjectResponse = new();
            List<CountryResponse> countries = await countryInfraContract.GetAllCountriesInfraAsync();
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

        public async Task<ApiObjectResponse> GetCountryByIdAppAsync(int countryId)
        {
            ApiObjectResponse apiObjectResponse = new();
            CountryResponse? countryResponse = await countryInfraContract.GetCountryByIdInfraAsync(countryId);
            if (countryResponse != null)
            {
                apiObjectResponse.StatusCode = HttpStatusCodes.Status200OK;
                apiObjectResponse.StatusMessage = HttpStatusMessages.Status200OK;
                apiObjectResponse.Data = countryResponse;
            }
            else
            {
                apiObjectResponse.StatusCode = HttpStatusCodes.Status204NoContent;
                apiObjectResponse.StatusMessage = HttpStatusMessages.Status204NoContent;
            }

            return apiObjectResponse;
        }

        public async Task<ApiBaseResponse> SaveCountryAppAsync(Country country)
        {
            ApiBaseResponse apiBaseResponse = new();
            int result = await countryInfraContract.SaveCountryInfraAsync(country);
            if (result > 0)
            {
                apiBaseResponse.StatusMessage = HttpStatusMessages.Status200OK;
                apiBaseResponse.StatusCode = HttpStatusCodes.Status200OK;
            }
            else
            {
                apiBaseResponse.StatusMessage = CustomHttpStatusMessages.UnXpectedError;
                apiBaseResponse.StatusCode = CustomHttpStatusCodes.UnXpectedError;
            }

            return apiBaseResponse;
        }
    }
}
