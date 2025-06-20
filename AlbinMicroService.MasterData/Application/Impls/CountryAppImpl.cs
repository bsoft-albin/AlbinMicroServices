using AlbinMicroService.Core;
using AlbinMicroService.Core.AbstractImpls;
using AlbinMicroService.MasterData.Application.Contracts;
using AlbinMicroService.MasterData.Domain.Models.Dtos;
using AlbinMicroService.MasterData.Domain.Models.Entities;
using AlbinMicroService.MasterData.Infrastructure.Contracts;

namespace AlbinMicroService.MasterData.Application.Impls
{
    public class CountryAppImpl(ICountryInfraContract countryInfraContract) : BaseAppImpls, ICountryAppContract
    {
        public async Task<ApiBaseResponse> DeleteCountryByIdAppAsync(int countryId)
        {
            ApiBaseResponse apiBaseResponse = new();
            GenericObjectSwitcher<bool> result = await countryInfraContract.DeleteCountryByIdInfraAsync(countryId);
            if (result.Data)
            {
                apiBaseResponse.StatusCode = HttpStatusCodes.Status204NoContent;
                apiBaseResponse.StatusMessage = HttpStatusMessages.Status204NoContent;
            }
            else if (!result.Data && result.IsErrorHappened)
            {
                return ProduceRuntimeErrorResponse(result);
            }
            else
            {
                apiBaseResponse.StatusCode = HttpStatusCodes.Status404NotFound;
                apiBaseResponse.StatusMessage = HttpStatusMessages.Status404NotFound;
            }

            return apiBaseResponse;
        }

        public async Task<ApiObjectResponse> GetAllCountriesAppAsync()
        {
            ApiObjectResponse apiObjectResponse = new();
            GenericObjectSwitcher<List<CountryResponse>> countries = await countryInfraContract.GetAllCountriesInfraAsync();
            if (countries.Data.Count > 0)
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
            GenericObjectSwitcherNull<CountryResponse> countryResponse = await countryInfraContract.GetCountryByIdInfraAsync(countryId);
            if (countryResponse.Data != null)
            {
                apiObjectResponse.StatusCode = HttpStatusCodes.Status200OK;
                apiObjectResponse.StatusMessage = HttpStatusMessages.Status200OK;
                apiObjectResponse.Data = countryResponse.Data;
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
            GenericObjectSwitcher<int> result = await countryInfraContract.SaveCountryInfraAsync(country);
            if (result.Data > Literals.Integer.Zero)
            {
                apiBaseResponse.StatusMessage = HttpStatusMessages.Status200OK;
                apiBaseResponse.StatusCode = HttpStatusCodes.Status200OK;
            }
            else if (result.IsErrorHappened && result.Data == Literals.Integer.Zero)
            {
                return ProduceRuntimeErrorResponse(result);
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
