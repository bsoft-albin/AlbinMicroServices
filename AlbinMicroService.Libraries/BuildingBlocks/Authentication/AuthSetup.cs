using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace AlbinMicroService.Libraries.BuildingBlocks.Authentication
{
    public static class AuthSetup
    {
        public static void AddAuthSetup(this IServiceCollection services)
        {
           services.AddAuthentication("Bearer")
            .AddJwtBearer("Bearer", options =>
            {
                options.RequireHttpsMetadata = false; // for dev mode only
                options.Authority = "http://localhost:9998"; // URL of IdentityServer
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateAudience = false
                };
            });

            services.AddAuthorization();
        }
    }
}
