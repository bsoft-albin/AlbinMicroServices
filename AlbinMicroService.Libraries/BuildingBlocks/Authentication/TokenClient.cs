using System.Net.Http.Json;
using AlbinMicroService.Core.Utilities;
using AlbinMicroService.Libraries.Common.Entities;

namespace AlbinMicroService.Libraries.BuildingBlocks.Authentication
{
    public interface ITokenClient
    {
        Task<TokenResponse> RefreshTokenAsync(RefreshTokenPayload refreshTokenPayload);
    }

    public class TokenClient(IHttpClientFactory _httpClientFactory) : ITokenClient
    {
        public async Task<TokenResponse> RefreshTokenAsync(RefreshTokenPayload refreshTokenPayload)
        {
            using HttpClient client = _httpClientFactory.CreateClient(Http.ClientNames.IdentityServer);

            HttpResponseMessage response = await client.PostAsync("http://localhost:9998/connect/token", new FormUrlEncodedContent(refreshTokenPayload.RefreshPayload));

            if (!response.IsSuccessStatusCode)
            {
                string errorDetails = await response.Content.ReadAsStringAsync();
                throw new Exception($"Failed to get refresh token. IdentityServer response: {errorDetails}");
            }

            TokenResult? result = await response.Content.ReadFromJsonAsync<TokenResult>();

            return new TokenResponse
            {
                AccessToken = result?.AccessToken,
                RefreshToken = result?.RefreshToken,
                ExpiresAt = DateTime.Now.AddSeconds(result?.ExpiresIn ?? 0)
            };
        }
    }
}
