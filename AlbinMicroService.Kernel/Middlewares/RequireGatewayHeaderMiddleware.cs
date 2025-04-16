using AlbinMicroService.Core.Utilities;
using Microsoft.AspNetCore.Http;

namespace AlbinMicroService.Kernel.Middlewares
{
    public class RequireGatewayHeaderMiddleware(RequestDelegate _next)
    {
        public async Task Invoke(HttpContext context)
        {
            string isTheRequestFromGateway = context.Request.Headers["X-From-Gateway"].FirstOrDefault() ?? "false";

            if (isTheRequestFromGateway != "true")
            {
                context.Response.StatusCode = HttpStatusCodes.Status403Forbidden;
                await context.Response.WriteAsync("Access Denied: Requests must come through Api Gateway");
                return;
            }

            await _next(context);
        }
    }
}
