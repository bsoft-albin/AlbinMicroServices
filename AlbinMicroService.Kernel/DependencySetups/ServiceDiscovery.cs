using AlbinMicroService.Core.Utilities;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;

namespace AlbinMicroService.Kernel.DependencySetups
{
    public static class ServiceDiscovery
    {
        public static void AddKernelServices(this IHostApplicationBuilder builder, IWebHostBuilder webHost, WebAppBuilderConfigTemplate configTemplate)
        {
            IServiceCollection Services = builder.Services;

            builder.Logging.ClearProviders(); // Remove all default logging providers

            if (!builder.Environment.IsDevelopment()) // Apply redirection only in Staging/Prod
            {
                builder.Services.AddHttpsRedirection(options =>
                {
                    options.HttpsPort = configTemplate.HttpsPort; // Redirect HTTP to HTTPS in non-dev environments
                });
            }

            // Configure Kestrel to listen on both HTTP and HTTPS
            webHost.ConfigureKestrel(options =>
            {
                options.AddServerHeader = false; // to remove the server: Kestrel from the api response headers. 
                options.ListenAnyIP(configTemplate.HttpPort); // HTTP
                if (configTemplate.IsHavingSSL || !builder.Environment.IsDevelopment())
                { // only enable HTTPS if IsHavingTLS is true or the mode will be staging or production
                    options.ListenAnyIP(configTemplate.HttpsPort, listenOptions =>
                    {
                        listenOptions.UseHttps(); // Enable HTTPS
                    });
                }
            });

            // Add Controllers to the container.
            Services.AddControllers();

            Services.AddEndpointsApiExplorer();
            Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc(configTemplate.ApiVersion, new()
                {
                    Title = configTemplate.ApiTitle,
                    Version = configTemplate.ApiVersion
                });
            });

        }

        public static void UseKernelMiddlewares(this IApplicationBuilder app, IHost host, IEndpointRouteBuilder route, IWebHostEnvironment env)
        {
            //Setting the Web App Mode.
            StaticMeths.SetGlobalWebAppMode(env.IsDevelopment(), env.IsStaging(), env.IsProduction());

            IConfiguration configuration = host.Services.GetRequiredService<IConfiguration>();

            bool isSwaggerEnabled = bool.Parse(configuration["Swagger:Enabled"] ?? "false"); // Check if Swagger is enabled in the configuration

            // Configure the HTTP request pipeline.

            if ((env.IsDevelopment() || env.IsStaging()) && isSwaggerEnabled) // only show Swagger in Development and Staging [for Production Or Live using this ["Swagger:Enabled"]]
            {
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", $"{env.ApplicationName[18..]} Api v1");
                    c.RoutePrefix = "swagger";
                });
            }

            // Redirect HTTP to HTTPS
            if (!env.IsDevelopment()) // Only force HTTPS in Staging and Production
            {
                app.UseHttpsRedirection();
            }
            else
            {
                app.UseDeveloperExceptionPage(); // Only in Development
            }

            app.UseSerilogRequestLogging(); // Enable Serilog request logging (Optional but recommended)

            app.UseAuthorization();

            route.MapControllers();

            host.Run();
        }

        public static void AddSeriloggings(this IHostApplicationBuilder builder, IHostBuilder host)
        {
            // Remove all default logging providers
            builder.Logging.ClearProviders();

            // Configuring Only Logger as Serilog
            host.UseSerilog((context, config) =>
            {
                config.ReadFrom.Configuration(context.Configuration);
            });
        }
    }
}
