using AlbinMicroService.Core.Utilities;
using Microsoft.AspNetCore.Http;

namespace AlbinMicroService.Kernel.Middlewares
{
    public class RequireGatewayHeaderMiddleware(RequestDelegate _next)
    {
        // Routes to Skip (give Routes in lower case only!!)
        private readonly HashSet<string> ExcludedRoutes =
        [
            "/api/home/is-running",
            "/api/users/get-custom-header",
            "/api/home/health",
            "/api/home/latest-version"
        ];

        public async Task Invoke(HttpContext context)
        {
            string path = context.Request.Path.Value?.TrimEnd('/').ToLower() ?? ""; // Here toLower Added for Case insensitive

            bool isExcluded = ExcludedRoutes.Contains(path);

            // Allow excluded routes to pass through
            if (!isExcluded)
            {
                //Enforce gateway header
                string isTheRequestFromGateway = context.Request.Headers["X-From-Gateway"].FirstOrDefault() ?? "false";

                if (isTheRequestFromGateway != "true")
                {
                    context.Response.StatusCode = HttpStatusCodes.Status403Forbidden;
                    await context.Response.WriteAsync("Access Denied: Requests must come through Api Gateway");
                    return;
                }
            }

            await _next(context);
        }
    }
}
