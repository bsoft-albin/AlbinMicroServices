using AlbinMicroService.Core;
using AlbinMicroService.MasterData.Domain.Models.Dtos;
using AlbinMicroService.MasterData.Domain.Models.Entities;

namespace AlbinMicroService.MasterData.Infrastructure.Contracts
{
    public interface ICountryInfraContract
    {
        Task<GenericObjectSwitcher<List<CountryResponse>>> GetAllCountriesInfraAsync();
        Task<GenericObjectSwitcherNull<CountryResponse>> GetCountryByIdInfraAsync(int countryId);
        Task<GenericObjectSwitcher<bool>> DeleteCountryByIdInfraAsync(int countryId);
        Task<GenericObjectSwitcher<int>> SaveCountryInfraAsync(Country country);
    }
}
