namespace AlbinMicroService.Kernel.Delegates
{
    public class GatewayHeaderHandler : DelegatingHandler
    {
        //This method will Header X-From-Gateway to all Requests that Come through Ocelot Gateway.
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            request.Headers.Add("X-From-Gateway", "true");
            return base.SendAsync(request, cancellationToken);
        }
    }
}
