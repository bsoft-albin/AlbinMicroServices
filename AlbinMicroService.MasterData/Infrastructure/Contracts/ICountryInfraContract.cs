using AlbinMicroService.MasterData.Domain.Models.Dtos;
using AlbinMicroService.MasterData.Domain.Models.Entities;

namespace AlbinMicroService.MasterData.Infrastructure.Contracts
{
    public interface ICountryInfraContract
    {
        Task<List<CountryResponse>> GetAllCountriesInfraAsync();
        Task<CountryResponse?> GetCountryByIdInfraAsync(int countryId);
        Task<bool?> DeleteCountryByIdInfraAsync(int countryId);
        Task<int> SaveCountryInfraAsync(Country country);
    }
}
