namespace AlbinMicroService.Kernel.Delegates
{
    public class GatewayHeaderHandler : DelegatingHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            request.Headers.Add("X-From-Gateway", "true");
            return base.SendAsync(request, cancellationToken);
        }
    }
}
