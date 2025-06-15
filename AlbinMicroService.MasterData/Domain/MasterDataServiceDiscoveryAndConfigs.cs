using AlbinMicroService.DataMappers.EntityFramework;
using AlbinMicroService.Kernel.DependencySetups;
using AlbinMicroService.MasterData.Application.Contracts;
using AlbinMicroService.MasterData.Application.Impls;
using AlbinMicroService.MasterData.Domain.ContextDb;
using AlbinMicroService.MasterData.Infrastructure.Contracts;
using AlbinMicroService.MasterData.Infrastructure.Impls;
using Microsoft.EntityFrameworkCore;

namespace AlbinMicroService.MasterData.Domain
{
    public static class MasterDataServiceDiscoveryAndConfigs
    {
        public static WebAppBuilderConfigTemplate AddDefaultServices(this WebApplicationBuilder builder)
        {
            WebAppBuilderConfigTemplate configs = ConfigurationSetup.BindSettings(builder.Configuration);

            // Add Kernel Services to the container.
            builder.AddKernelServices(builder.WebHost, configs);

            return configs;
        }

        public static WebApplicationBuilder AddDatabaseServices(this WebApplicationBuilder builder)
        {
            // Add DbContext with MySQL
            builder.Services.AddDbContext<MasterDataDbContext>(options =>
                options.UseMySql(
                    builder.Configuration.GetConnectionString("MySqlConnection"),
                    ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("MySqlConnection"))
                )
            );

            // 👇 This maps DbContext (base) to MasterDataDbContext (your actual class)
            builder.Services.AddScoped<DbContext>(provider => provider.GetRequiredService<MasterDataDbContext>());

            return builder;
        }

        public static WebApplicationBuilder AddCustomServices(this WebApplicationBuilder builder)
        {
            builder.AddSeriloggings(builder.Host);

            return builder;
        }

        public static WebApplicationBuilder AddMasterDataServices(this WebApplicationBuilder builder)
        {
            builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

            builder.Services.AddScoped<ICountryInfraContract, CountryInfraImpl>();
            builder.Services.AddScoped<ICountryAppContract, CountryAppImpl>();
            builder.Services.AddScoped<IDynamicMeths, DynamicMeths>();

            return builder;
        }
    }
}
