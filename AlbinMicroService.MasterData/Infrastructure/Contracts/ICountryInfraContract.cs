using AlbinMicroService.MasterData.Domain.Models.Entities;

namespace AlbinMicroService.MasterData.Infrastructure.Contracts
{
    public interface ICountryInfraContract
    {
        Task<List<Country>> GetAllCountriesInfraAsync();
    }
}
