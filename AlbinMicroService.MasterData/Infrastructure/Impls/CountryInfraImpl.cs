using AlbinMicroService.Core;
using AlbinMicroService.Core.AbstractImpls;
using AlbinMicroService.DataMappers.EntityFramework;
using AlbinMicroService.MasterData.Domain.Models.Dtos;
using AlbinMicroService.MasterData.Domain.Models.Entities;
using AlbinMicroService.MasterData.Infrastructure.Contracts;

namespace AlbinMicroService.MasterData.Infrastructure.Impls
{
    public class CountryInfraImpl(IGenericRepository<Country> countryRepo, ILogger<CountryInfraImpl> logger) : BaseInfraImpls, ICountryInfraContract
    {
        public async Task<GenericObjectSwitcher<bool>> DeleteCountryByIdInfraAsync(int countryId)
        {
            GenericObjectSwitcher<bool> genericObjectSwitcher = new();
            try
            {
                Country? country = await countryRepo.GetByIdAsync(countryId);

                if (country != null)
                {
                    country.DeletedAt = StaticProps.AppDateTimeNow;
                    country.IsDeleted = Literals.Boolean.True;

                    countryRepo.Update(country);
                    await countryRepo.SaveChangesAsync();

                    genericObjectSwitcher.DataSwitcher = Literals.Boolean.True;
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error");
                //genericObjectSwitcher.ErrorData = ex.ToErrorObject();
                //genericObjectSwitcher.Error = ex.Message;
                //genericObjectSwitcher.IsErrorHappened = Literals.Boolean.True;
                //genericObjectSwitcher.DataSwitcher = Literals.Boolean.False;
            }

            return genericObjectSwitcher;
        }

        public async Task<List<CountryResponse>> GetAllCountriesInfraAsync()
        {
            List<CountryResponse> countryList = [];
            try
            {
                IEnumerable<Country> data = await countryRepo.GetAllAsync();
                countryList = [.. data.Where(w => !w.IsDeleted)
                    .Select(s => new CountryResponse
                    {
                        Code = s.Code,
                        Name = s.Name,
                        Id = s.Id,
                        DialCode = s.DialCode
                    })];
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error");
            }

            return countryList;
        }

        public async Task<CountryResponse?> GetCountryByIdInfraAsync(int countryId)
        {
            try
            {
                Country? country = await countryRepo.GetByIdAsync(countryId);
                if (country != null)
                {
                    return new CountryResponse { Code = country.Code, DialCode = country.DialCode, Id = country.Id, Name = country.Name };
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error");
                return null;
            }
        }

        public async Task<int> SaveCountryInfraAsync(Country country)
        {
            int result = 0;
            try
            {
                if (country.Id > 0)
                {
                    countryRepo.Update(country);
                }
                else
                {
                    await countryRepo.AddAsync(country);
                }
                if (await countryRepo.SaveChangesAsync() > 0)
                {
                    result = country.Id;
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error");
            }

            return result;
        }
    }
}
