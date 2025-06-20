using AlbinMicroService.Core;
using AlbinMicroService.Core.AbstractImpls;
using AlbinMicroService.DataMappers.EntityFramework;
using AlbinMicroService.DataMappers.Utilities;
using AlbinMicroService.MasterData.Domain.Models.Dtos;
using AlbinMicroService.MasterData.Domain.Models.Entities;
using AlbinMicroService.MasterData.Infrastructure.Contracts;
using Microsoft.EntityFrameworkCore;

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

                    genericObjectSwitcher.Data = Literals.Boolean.True;
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error");

                return genericObjectSwitcher.DoExceptionFlow(ex);
            }

            return genericObjectSwitcher;
        }

        public async Task<GenericObjectSwitcher<List<CountryResponse>>> GetAllCountriesInfraAsync()
        {
            GenericObjectSwitcher<List<CountryResponse>> genericObjectSwitcher = new();

            try
            {
                List<CountryResponse> countryList = await countryRepo.GetAllAsQueryable(Q => !Q.IsDeleted).
                                Select(s => new CountryResponse
                                {
                                    Code = s.Code,
                                    Name = s.Name,
                                    Id = s.Id,
                                    DialCode = s.DialCode
                                }).ToListAsync();

                if (countryList.Count > 0)
                {
                    genericObjectSwitcher.Data = countryList;
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error");

                return genericObjectSwitcher.DoExceptionFlow(ex);
            }

            return genericObjectSwitcher;
        }

        public async Task<GenericObjectSwitcherNull<CountryResponse>> GetCountryByIdInfraAsync(int countryId)
        {
            GenericObjectSwitcherNull<CountryResponse> genericObjectSwitcher = new();
            try
            {
                Country? country = await countryRepo.GetByIdAsync(countryId);
                if (country != null)
                {
                    genericObjectSwitcher.Data = new CountryResponse { Code = country.Code, DialCode = country.DialCode, Id = country.Id, Name = country.Name };
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error");

                return genericObjectSwitcher.DoExceptionFlow(ex);
            }

            return genericObjectSwitcher;
        }

        public async Task<GenericObjectSwitcher<int>> SaveCountryInfraAsync(Country country)
        {
            GenericObjectSwitcher<int> genericObjectSwitcher = new();
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
                    genericObjectSwitcher.Data = country.Id;
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error");

                return genericObjectSwitcher.DoExceptionFlow(ex);
            }

            return genericObjectSwitcher;
        }
    }
}
