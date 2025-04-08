using AlbinMicroService.Gateway.Ocelot;

namespace AlbinMicroService.Gateway
{
    public static class GatewayServiceDiscoveryAndConfigs
    {
        public static void AddDefaultServices(this WebApplicationBuilder builder)
        {
            int HTTP_PORT = int.Parse(builder.Configuration["Configs:HttpPort"] ?? "9001");
            int HTTPS_PORT = int.Parse(builder.Configuration["Configs:HttpsPort"] ?? "9002");
            bool IsRunsInContainer = bool.Parse(builder.Configuration["Configs:IsRunningInContainer"] ?? "false");
            bool IsHavingTLS = bool.Parse(builder.Configuration["Configs:IsHavingSSL"] ?? "false");

            // adding Ocelot configuration to the builder
            builder.AddOcelotConfigurations();

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
                options.AddServerHeader = false; // Removes "Server: Kestrel" from response headers
                options.ListenAnyIP(HTTP_PORT); // HTTP
                if (IsHavingTLS || !builder.Environment.IsDevelopment())
                { // only enable HTTPS if IsHavingTLS is true or the mode will be staging or production
                    options.ListenAnyIP(HTTPS_PORT, listenOptions =>
                    {
                        listenOptions.UseHttps(); // Enable HTTPS
                    });
                }
            });
        }
    }
}
