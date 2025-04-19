using AlbinMicroService.Libraries.BuildingBlocks.Authentication;
using Microsoft.AspNetCore.Http;
using System.Net.Http.Headers;

namespace AlbinMicroService.Kernel.Delegates
{
    public class TokenHandler(ITokenStoreService _tokenStore, IHttpContextAccessor _httpContextAccessor) : DelegatingHandler
    {
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var userId = _httpContextAccessor.HttpContext.User.FindFirst("sub")?.Value;

            if (!string.IsNullOrWhiteSpace(userId))
            {
                var token = await _tokenStore.GetValidAccessTokenAsync(userId);
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            return await base.SendAsync(request, cancellationToken);
        }

    }
}
