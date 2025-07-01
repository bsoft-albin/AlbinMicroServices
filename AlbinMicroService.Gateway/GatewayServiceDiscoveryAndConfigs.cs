using System.IO.Compression;
using AlbinMicroService.Core.Utilities;
using AlbinMicroService.Gateway.Ocelot;
using AlbinMicroService.Kernel.Delegates;
using AlbinMicroService.Kernel.DependencySetups;
using AlbinMicroService.Libraries.BuildingBlocks.Authentication;
using Microsoft.AspNetCore.ResponseCompression;

namespace AlbinMicroService.Gateway
{
    public static class GatewayServiceDiscoveryAndConfigs
    {
        public static WebAppBuilderConfigTemplate AddDefaultServices(this WebApplicationBuilder builder)
        {
            WebAppBuilderConfigTemplate configTemplate = ConfigurationSetup.BindSettings(builder.Configuration);

            //you still need to call AddControllers() (or AddMvcCore()), even if you don’t have any controllers, because Swagger and SwaggerForOcelot depend
            //on the IApiDescriptionProvider and related services, which are only registered when MVC services are added.
            builder.Services.AddControllers(); //required for Swagger's internal deps
            builder.Services.AddEndpointsApiExplorer(); //enables [ApiExplorer] features
            builder.Services.AddSwaggerGen();

            builder.Services.AddHttpContextAccessor();
            builder.Services.AddMemoryCache();
            builder.Services.AddSingleton<ITokenStoreService, TokenStoreService>();
            builder.Services.AddTransient<TokenHandler>();

            //Adding Response Compressions to DI. (Both Brotli and GZip)
            builder.Services.AddResponseCompression(options =>
            {
                options.EnableForHttps = true;
                options.Providers.Add<GzipCompressionProvider>();
                options.Providers.Add<BrotliCompressionProvider>();
            });

            builder.Services.Configure<GzipCompressionProviderOptions>(options =>
            {
                options.Level = CompressionLevel.Optimal;
            });

            builder.Services.Configure<BrotliCompressionProviderOptions>(options =>
            {
                options.Level = CompressionLevel.Optimal;
            });

            // adding Ocelot configuration to the builder
            builder.AddOcelotConfigurations();

            //adding our Token Client
            builder.Services.AddSingleton<ITokenClient, TokenClient>();

            builder.Services.AddSwaggerForOcelot(builder.Configuration);

            if (!builder.Environment.IsDevelopment()) // Apply redirection only in Staging/Prod
            {
                builder.Services.AddHttpsRedirection(options =>
                {
                    options.HttpsPort = configTemplate.HttpsPort; // Redirect HTTP to HTTPS in non-dev environments
                });
            }

            // Configure Kestrel to listen on both HTTP and HTTPS
            builder.WebHost.ConfigureKestrel(options =>
            {
                options.AddServerHeader = false; // Removes "Server: Kestrel" from response headers
                options.ListenAnyIP(configTemplate.HttpPort); // HTTP
                if (configTemplate.IsHavingSSL || !builder.Environment.IsDevelopment())
                { // only enable HTTPS if IsHavingTLS is true or the mode will be staging or production
                    options.ListenAnyIP(configTemplate.HttpsPort, listenOptions =>
                    {
                        listenOptions.UseHttps(); // Enable HTTPS
                    });
                }
            });

            return configTemplate;
        }
    }
}
