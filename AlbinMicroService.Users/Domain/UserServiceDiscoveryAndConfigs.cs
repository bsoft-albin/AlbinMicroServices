using AlbinMicroService.DataMappers.Dapper;
using AlbinMicroService.DataMappers.RawADO;
using AlbinMicroService.Kernel.DependencySetups;
using AlbinMicroService.Libraries.BuildingBlocks.Authentication;
using AlbinMicroService.Users.Application.Contracts;
using AlbinMicroService.Users.Application.Impls;
using AlbinMicroService.Users.Domain.Contracts;
using AlbinMicroService.Users.Domain.Impls;
using AlbinMicroService.Users.Infrastructure.Contracts;
using AlbinMicroService.Users.Infrastructure.Impls;
using MySql.Data.MySqlClient;

namespace AlbinMicroService.Users.Domain
{
    public static class UserServiceDiscoveryAndConfigs
    {
        public static WebAppConfigs AddDefaultServices(this WebApplicationBuilder builder)
        {
            WebAppConfigs configs = ConfigurationSetup.BindSettings(builder.Configuration);

            builder.Services.AddSingleton<ITokenClient, TokenClient>();

            // Add Kernel Services to the container.
            builder.AddKernelServices(builder.WebHost, configs);

            return configs;
        }

        public static WebApplicationBuilder AddDatabaseServices(this WebApplicationBuilder builder)
        {
            string mysqlConnection = builder.Configuration.GetConnectionString(DatabaseTypes.MySql)!;

            builder.Services.AddScoped<IDapperHelper>(sp => new DapperHelper(mysqlConnection));
            builder.Services.AddScoped(db => new DbTransactionHelper(MySqlClientFactory.Instance, mysqlConnection));

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
