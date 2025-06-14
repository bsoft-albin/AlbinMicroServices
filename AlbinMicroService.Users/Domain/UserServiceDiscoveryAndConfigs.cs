using AlbinMicroService.DataMappers.Dapper;
using AlbinMicroService.Kernel.DependencySetups;
using AlbinMicroService.Users.Application.Contracts;
using AlbinMicroService.Users.Application.Impls;
using AlbinMicroService.Users.Domain.Contracts;
using AlbinMicroService.Users.Domain.Impls;
using AlbinMicroService.Users.Infrastructure.Contracts;
using AlbinMicroService.Users.Infrastructure.Impls;

namespace AlbinMicroService.Users.Domain
{
    public static class UserServiceDiscoveryAndConfigs
    {
        public static WebAppBuilderConfigTemplate AddDefaultServices(this WebApplicationBuilder builder)
        {
            WebAppBuilderConfigTemplate configs = ConfigurationSetup.BindSettings(builder.Configuration);

            //adding one Named Http Client to the DI. (if you want ,move this to specific service..
            builder.Services.AddHttpClient("IdentityServerHttpClient", client =>
            {
                client.BaseAddress = new Uri("http://localhost:9998/");
                client.DefaultRequestHeaders.Add("Accept", "application/json");
                client.DefaultRequestHeaders.Add("User-Agent", "Albin-MicroServices"); // Optional but recommended
                client.Timeout = TimeSpan.FromSeconds(60);
            });

            // Add Kernel Services to the container.
            builder.AddKernelServices(builder.WebHost, configs);

            return configs;
        }

        public static WebApplicationBuilder AddDatabaseServices(this WebApplicationBuilder builder)
        {
            string? connectionString = builder.Configuration.GetConnectionString(DatabaseTypes.MySql);
            builder.Services.AddScoped<IDapperHelper>(sp => new DapperHelper(connectionString ?? string.Empty));

            return builder;
        }

        public static WebApplicationBuilder AddCustomServices(this WebApplicationBuilder builder)
        {
            builder.AddSeriloggings(builder.Host);

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
