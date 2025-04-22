namespace AlbinMicroService.Identity
{
    // Legacy Way of calling the program
    public class Program
    {
        public static void Main(string[] args)
        {
            WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

            // Add OAuth services to the container.
            builder.Services.AddIdentityServer()
                .AddInMemoryClients(Config.Clients)
                .AddInMemoryApiScopes(Config.ApiScopes)
                .AddInMemoryApiResources(Config.ApiResources)
                .AddInMemoryIdentityResources(Config.IdentityResources)
                .AddTestUsers(Config.Users)
                .AddDeveloperSigningCredential(); // for dev mode only

            WebApplication app = builder.Build();

            //using OAuth2 with Jwt
            app.UseIdentityServer();

            app.MapGet("/", () => "Identity Server is running!");
            app.Run();
        }
    }
}
