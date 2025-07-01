using System.Net.Http.Json;
using AlbinMicroService.Core.Utilities;
using AlbinMicroService.Libraries.Common.Entities;
using static AlbinMicroService.Core.Utilities.ApiAuthorization;

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
            using HttpClient client = _httpClientFactory.CreateClient(Http.ClientNames.IdentityServer);

            Dictionary<string, string> form = new()
            {
                { TokenRequestKeys.grant_type, GrantTypes.refresh_token },
                { TokenRequestKeys.client_id, SystemClientIds.ADMIN },
                { TokenRequestKeys.refresh_token, refreshToken }
            };

            HttpResponseMessage response = await client.PostAsync("http://localhost:9998/connect/token", new FormUrlEncodedContent(form));

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Failed to get refresh token.");
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
