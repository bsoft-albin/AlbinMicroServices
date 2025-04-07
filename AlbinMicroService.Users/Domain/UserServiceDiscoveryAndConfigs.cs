using AlbinMicroService.Kernel.DependencySetups;
using AlbinMicroService.Users.Application.Contracts;
using AlbinMicroService.Users.Application.Impls;
using AlbinMicroService.Users.Domain.Contracts;
using AlbinMicroService.Users.Domain.Impls;
using AlbinMicroService.Users.Infrastructure.Contracts;
using AlbinMicroService.Users.Infrastructure.Impls;
using Serilog;

namespace AlbinMicroService.Users.Domain
{
    public static class UserServiceDiscoveryAndConfigs
    {
        public static WebApplicationBuilder AddDefaultServices(this WebApplicationBuilder builder)
        {
            WebAppBuilderConfigTemplate configs = ConfigurationSetup.BindSettings(builder.Configuration);

            if (!builder.Environment.IsDevelopment()) // Apply redirection only in Staging/Prod
            {
                builder.Services.AddHttpsRedirection(options =>
                {
                    options.HttpsPort = configs.HttpsPort; // Redirect HTTP to HTTPS in non-dev environments
                });
            }

            // Configure Kestrel to listen on both HTTP and HTTPS
            builder.WebHost.ConfigureKestrel(options =>
            {
                options.AddServerHeader = false; // to remove the server: Kestrel from the api response headers. 
                options.ListenAnyIP(configs.HttpPort); // HTTP
                if (configs.IsHavingSSL || !builder.Environment.IsDevelopment())
                { // only enable HTTPS if IsHavingTLS is true or the mode will be staging or production
                    options.ListenAnyIP(configs.HttpsPort, listenOptions =>
                    {
                        listenOptions.UseHttps(); // Enable HTTPS
                    });
                }
            });

            // Add Kernel Services to the container.
            builder.Services.AddKernelServices();

            return builder;
        }

        public static WebApplicationBuilder AddDatabaseServices(this WebApplicationBuilder builder)
        {
            string connectionString = builder.Configuration.GetConnectionString(DatabaseTypes.MySql) ?? string.Empty;
            //builder.Services.AddScoped<IMySqlMapper>(sp => new MySqlMapper(connectionString));

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
