using AlbinMicroService.Core.Utilities;
using AlbinMicroService.DataMappers.Dapper;
using Duende.IdentityServer.Validation;

namespace AlbinMicroService.Identity
{
    // Legacy Way of calling the program
    public static class Program
    {
        public static void Main(string[] args)
        {
            WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

            // Add OAuth 2.0 services to the container.
            IIdentityServerBuilder identityServer = builder.Services.AddIdentityServer()
                .AddResourceOwnerValidator<CustomResourceOwnerPasswordValidator>()
                .AddInMemoryClients(IdentityServerConfigs.Clients)
                .AddInMemoryApiScopes(IdentityServerConfigs.ApiScopes)
                .AddInMemoryApiResources(IdentityServerConfigs.ApiResources)
                .AddInMemoryIdentityResources(IdentityServerConfigs.IdentityResources);
                //.AddTestUsers(IdentityServerConfigs.Users); // commneted because Db fetching added for users

            if (builder.Environment.IsDevelopment())
            {
                identityServer.AddDeveloperSigningCredential(); // for dev mode only
            }

            // Service Registrations
            builder.Services.AddSingleton<IDynamicMeths, DynamicMeths>();
            builder.Services.AddScoped<IDapperHelper>(sp => new DapperHelper(builder.Configuration.GetConnectionString("AuthDb") ?? string.Empty));
            builder.Services.AddScoped<IResourceOwnerPasswordValidator, CustomResourceOwnerPasswordValidator>();
            builder.Services.AddScoped<IUserService, UserService>();

            WebApplication app = builder.Build();

            //using OAuth 2.0 Authentication with Jwt Authorization Token
            app.UseIdentityServer();

            app.MapGet("/", () => "Identity Server is running!!!");

            app.Run();
        }
    }
}
