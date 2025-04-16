namespace AlbinMicroService.Kernel.Delegates
{
    public class GatewayHeaderHandler : DelegatingHandler
    {
        // Define the paths you want to skip
        private readonly List<string> ExcludedRoutes =
        [
        "/is-running",
        "/user/api/users-service/data-check"
        ];

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var path = request.RequestUri?.AbsolutePath?.TrimEnd('/').ToLower() ?? "";

            if (!ExcludedRoutes.Contains(path))
            {
                request.Headers.Add("X-From-Gateway", "true");
            }

            return base.SendAsync(request, cancellationToken);
        }
        //protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        //{
        //    request.Headers.Add("X-From-Gateway", "true");
        //    return base.SendAsync(request, cancellationToken);
        //}
    }
}
