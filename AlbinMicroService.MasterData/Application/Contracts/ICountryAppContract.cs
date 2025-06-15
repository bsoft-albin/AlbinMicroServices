using AlbinMicroService.Core;

namespace AlbinMicroService.MasterData.Application.Contracts
{
    public interface ICountryAppContract
    {
        Task<ApiObjectResponse> GetAllCountriesAppAsync();
    }
}
