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
                .AddInMemoryClients(Config.Clients)
                .AddInMemoryApiScopes(Config.ApiScopes)
                .AddInMemoryApiResources(Config.ApiResources)
                .AddInMemoryIdentityResources(Config.IdentityResources)
                .AddTestUsers(Config.Users);

            if (builder.Environment.IsDevelopment())
            {
                identityServer.AddDeveloperSigningCredential(); // for dev mode only
            }

            WebApplication app = builder.Build();

            //using OAuth 2.0 Authentication with Jwt Authorization Token
            app.UseIdentityServer();

            app.MapGet("/", () => "Identity Server is running!");

            app.Run();
        }
    }
}
