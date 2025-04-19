using AlbinMicroService.Libraries.Common.Entities;
using System.Net.Http.Json;

namespace AlbinMicroService.Libraries.BuildingBlocks.Authentication
{
    public interface ITokenClient
    {
        Task<TokenResponse> RefreshTokenAsync(string refreshToken);
    }

    public class TokenClient(IHttpClientFactory _httpClientFactory) : ITokenClient
    {
        public async Task<TokenResponse> RefreshTokenAsync(string refreshToken)
        {
            HttpClient client = _httpClientFactory.CreateClient(); // You can use a named client if you want

            Dictionary<string, string> form = new()
        {
            { "grant_type", "refresh_token" },
            { "client_id", "albin-microservice-client" },
            { "client_secret", "albin-microservice_client_secret" },
            { "refresh_token", refreshToken }
        };

            HttpResponseMessage response = await client.PostAsync("http://localhost:9998/connect/token", new FormUrlEncodedContent(form));

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Failed to refresh token.");
            }

            TokenResult result = await response.Content.ReadFromJsonAsync<TokenResult>();

            return new TokenResponse
            {
                AccessToken = result.AccessToken,
                RefreshToken = result.RefreshToken,
                ExpiresAt = DateTime.UtcNow.AddSeconds(result.ExpiresIn)
            };
        }
    }
}
