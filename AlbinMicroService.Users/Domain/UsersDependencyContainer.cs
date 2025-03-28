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
            int HTTP_PORT = int.Parse(builder.Configuration["Configs:HttpPort"] ?? "5001");
            int HTTPS_PORT = int.Parse(builder.Configuration["Configs:HttpsPort"] ?? "5001");

            builder.WebHost.ConfigureKestrel(options =>
            {
                options.ListenAnyIP(80);  // Ensure the app listens on port 80
            });

            // Add services to the container.
            builder.Services.AddControllers();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

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
            // Remove default logging providers
            builder.Logging.ClearProviders();

            // Configure Serilog
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
