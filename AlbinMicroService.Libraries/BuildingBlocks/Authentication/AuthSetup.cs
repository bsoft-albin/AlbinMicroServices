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


            //services.AddAuthorization();


            //Authorization Policies
            services.AddAuthorizationBuilder()

            // 🔐 Scope-based Policies
            .AddPolicy("ReadMasterScope", policy =>
                policy.RequireClaim("scope", "master.read"))

            .AddPolicy("WriteMasterScope", policy =>
                policy.RequireClaim("scope", "master.write"))

            .AddPolicy("UserReadScope", policy =>
                policy.RequireClaim("scope", "user.read"))

            .AddPolicy("UserWriteScope", policy =>
                policy.RequireClaim("scope", "user.write"))

            .AddPolicy("AdminReadScope", policy =>
                policy.RequireClaim("scope", "admin.read"))

            .AddPolicy("AdminWriteScope", policy =>
                policy.RequireClaim("scope", "admin.write"))

            // 🧑 Role-based Policies
            .AddPolicy("AdminOnly", policy =>
                policy.RequireRole("admin"))

            .AddPolicy("UserOnly", policy =>
                policy.RequireRole("user"))

            .AddPolicy("ManagerOnly", policy =>
                policy.RequireRole("manager"))

            .AddPolicy("SuperAdminOnly", policy =>
                policy.RequireRole("superadmin"));
        }
    }
}
