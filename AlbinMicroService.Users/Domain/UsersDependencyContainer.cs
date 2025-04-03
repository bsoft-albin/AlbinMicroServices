using AlbinMicroService.Core.Repository;
using AlbinMicroService.Users.Application.Contracts;
using AlbinMicroService.Users.Application.Impls;
using AlbinMicroService.Users.Domain.Contracts;
using AlbinMicroService.Users.Domain.Impls;
using AlbinMicroService.Users.Infrastructure.Contracts;
using AlbinMicroService.Users.Infrastructure.Impls;
using Serilog;

namespace AlbinMicroService.Users.Domain
{
    public static class UsersDependencyContainer
    {
        public static WebApplicationBuilder AddDefaultServices(this WebApplicationBuilder builder)
        {
            int HTTP_PORT = int.Parse(builder.Configuration["Configs:HttpPort"] ?? "8001");
            int HTTPS_PORT = int.Parse(builder.Configuration["Configs:HttpsPort"] ?? "8002");
            bool IsRunsInContainer = bool.Parse(builder.Configuration["Configs:IsRunningInContainer"] ?? "false");

            if (!builder.Environment.IsDevelopment()) // Apply redirection only in Staging/Prod
            {
                builder.Services.AddHttpsRedirection(options =>
                {
                    options.HttpsPort = HTTPS_PORT; // Redirect HTTP to HTTPS in non-dev environments
                });
            }

            // Configure Kestrel to listen on both HTTP and HTTPS
            builder.WebHost.ConfigureKestrel(options =>
            {
                options.AddServerHeader = false; // to remove the server: Kestrel from the api response headers. 
                options.ListenAnyIP(HTTP_PORT); // HTTP
                options.ListenAnyIP(HTTPS_PORT, listenOptions =>
                {
                    listenOptions.UseHttps(); // Enable HTTPS
                });
            });

            // Add services to the container.
            builder.Services.AddControllers();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new() { Title = "Users Api", Version = "v1" });
            });

            return builder;
        }
        public static WebApplicationBuilder AddDatabaseServices(this WebApplicationBuilder builder)
        {
            string connectionString = builder.Configuration.GetConnectionString(DatabaseTypes.MySql) ?? string.Empty;
            builder.Services.AddScoped<IMySqlMapper>(sp => new MySqlMapper(connectionString));

            return builder;
        }
        public static WebApplicationBuilder AddCustomServices(this WebApplicationBuilder builder)
        {
            // Remove all default logging providers
            builder.Logging.ClearProviders();

            // Configuring Only Logger as Serilog
            builder.Host.UseSerilog((context, config) =>
            {
                config.ReadFrom.Configuration(context.Configuration);
            });

            return builder;
        }

        public static WebApplicationBuilder AddUserServices(this WebApplicationBuilder builder)
        {
            builder.Services.AddScoped<IUsersAppContract, UsersAppImpl>();
            builder.Services.AddScoped<IUsersDomainContract, UsersDomainImpl>();
            builder.Services.AddScoped<IUsersInfraContract, UsersInfraImpl>();
            builder.Services.AddScoped<IDynamicMeths, DynamicMeths>();

            return builder;
        }
    }
}
