using AlbinMicroService.Libraries.Common.Entities;
using Microsoft.Extensions.Caching.Memory;

namespace AlbinMicroService.Libraries.BuildingBlocks.Authentication
{
    public interface ITokenStoreService
    {
        Task<string> GetValidAccessTokenAsync(string userId);
    }

    public class TokenStoreService(IMemoryCache _cache, ITokenClient _tokenClient) : ITokenStoreService
    {
        public async Task<string> GetValidAccessTokenAsync(string userId)
        {
            if (_cache.TryGetValue<TokenResponse>($"token_{userId}", out var token))
            {
                if (token.ExpiresAt > DateTime.Now)
                    return token.AccessToken;

                // Refresh if expired
                var refreshed = await _tokenClient.RefreshTokenAsync(token.RefreshToken);
                _cache.Set($"token_{userId}", refreshed, refreshed.ExpiresAt - DateTime.Now);
                return refreshed.AccessToken;
            }

            throw new UnauthorizedAccessException("Token not found for user.");
        }
    }
}
