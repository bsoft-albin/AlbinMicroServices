using AlbinMicroService.Core.Utilities;
using AlbinMicroService.Gateway.Ocelot;
using AlbinMicroService.Kernel.DependencySetups;

namespace AlbinMicroService.Gateway
{
    public static class GatewayServiceDiscoveryAndConfigs
    {
        public static WebAppBuilderConfigTemplate AddDefaultServices(this WebApplicationBuilder builder)
        {
            WebAppBuilderConfigTemplate configTemplate = ConfigurationSetup.BindSettings(builder.Configuration);

            // adding Ocelot configuration to the builder
            builder.AddOcelotConfigurations();

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
