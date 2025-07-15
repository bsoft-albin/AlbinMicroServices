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
        public static WebAppConfigs AddDefaultServices(this WebApplicationBuilder builder)
        {
            WebAppConfigs configs = ConfigurationSetup.BindSettings(builder.Configuration);

            // Add Kernel Services to the container.
            builder.AddKernelServices(builder.WebHost, configs);

            return configs;
        }

        public static WebApplicationBuilder AddDatabaseServices(this WebApplicationBuilder builder)
        {
            // Add DbContext with MySQL
            builder.Services.AddDbContext<MasterDataDbContext>((serviceProvider, options) =>
            {
                string connectionString = builder.Configuration.GetConnectionString("MySqlConnection")!;

                options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString), mySqlOptions =>
                {
                    mySqlOptions
                        .UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery) // Prevents cartesian explosion on includes
                        .MaxBatchSize(100); // Improve SaveChanges batching
                });

                //Get logger factory that includes Serilog
                ILoggerFactory loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();
                options
                    .UseLoggerFactory(loggerFactory)
                    .EnableSensitiveDataLogging()
                    .EnableDetailedErrors();
            });

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
