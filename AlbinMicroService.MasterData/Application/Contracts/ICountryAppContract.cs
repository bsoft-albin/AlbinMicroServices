using AlbinMicroService.Core;
using AlbinMicroService.MasterData.Domain.Models.Entities;

namespace AlbinMicroService.MasterData.Application.Contracts
{
    public interface ICountryAppContract
    {
        Task<ApiObjectResponse> GetAllCountriesAppAsync();
        Task<ApiBaseResponse> DeleteCountryByIdAppAsync(int countryId);
        Task<ApiObjectResponse> GetCountryByIdAppAsync(int countryId);
        Task<ApiBaseResponse> SaveCountryAppAsync(Country country);
    }
}
