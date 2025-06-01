using AlbinMicroService.Core.Utilities;
using AlbinMicroService.DataMappers.Dapper;
using Duende.IdentityServer.Validation;
using MongoDB.Driver.Core.Configuration;

namespace AlbinMicroService.Identity
{
    // Legacy Way of calling the program
    public class Program
    {
        public static void Main(string[] args)
        {
            WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

            // Add OAuth 2.0 services to the container.
            IIdentityServerBuilder identityServer = builder.Services.AddIdentityServer()
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
            builder.Services.AddScoped<IDapperHelper>(sp => new DapperHelper("Server=localhost;Port=3306;Database=users;Uid=albin;Pwd=billy;"));
            builder.Services.AddScoped<IResourceOwnerPasswordValidator, CustomResourceOwnerPasswordValidator>();
            builder.Services.AddScoped<IUserService, UserService>();

            WebApplication app = builder.Build();

            //using OAuth 2.0 Authentication with Jwt Authorization Token
            app.UseIdentityServer();

            app.MapGet("/", () => "Identity Server is running!");

            app.Run();
        }
    }
}
