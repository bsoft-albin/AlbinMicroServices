using AlbinMicroService.DataMappers.EntityFramework;
using AlbinMicroService.MasterData.Domain.Models.Entities;
using AlbinMicroService.MasterData.Infrastructure.Contracts;

namespace AlbinMicroService.MasterData.Infrastructure.Impls
{
    public class CountryInfraImpl(IGenericRepository<Country> countryRepo, ILogger<CountryInfraImpl> logger) : ICountryInfraContract
    {
        public async Task<List<Country>> GetAllCountriesInfraAsync()
        {
            List<Country> countryList = [];
            try
            {
                IEnumerable<Country> data = await countryRepo.GetAllAsync();
                countryList = data.Where(w => !w.IsDeleted).ToList();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error");
            }

            return countryList;
        }
    }
}
