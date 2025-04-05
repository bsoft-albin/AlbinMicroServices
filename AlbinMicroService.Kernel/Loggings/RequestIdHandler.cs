namespace AlbinMicroService.Kernel.Loggings
{
    public class RequestIdHandler : DelegatingHandler
    {
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            request.Headers.TryAddWithoutValidation("X-Request-Id", Guid.NewGuid().ToString());
            return await base.SendAsync(request, cancellationToken);
        }
    }
}
