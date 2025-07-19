using AlbinMicroService.Core.Utilities;
using AlbinMicroService.Kernel.Middlewares;
using AlbinMicroService.Libraries.BuildingBlocks.Authentication;
using AlbinMicroService.Libraries.BuildingBlocks.BackgroundServices;
using Asp.Versioning.ApiExplorer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;

namespace AlbinMicroService.Kernel.DependencySetups
{
    public static class ServiceDiscovery
    {
        public static void AddKernelServices(this IHostApplicationBuilder builder, IWebHostBuilder webHost, WebAppConfigs configTemplate)
        {
            IServiceCollection Services = builder.Services;

            if (!builder.Environment.IsDevelopment()) // Apply redirection only in Staging/Prod
            {
                Services.AddHttpsRedirection(options =>
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

            //adding (Default) HttpClient's to the DI.
            Services.AddHttpClient();

            //adding one Named Http Client to the DI.
            Services.AddHttpClient(Http.ClientNames.IdentityServer, client =>
            {
                client.BaseAddress = new Uri(Http.BaseUri.IdentityServer_Http);
                client.DefaultRequestHeaders.Add("Accept", "application/json");
                client.DefaultRequestHeaders.Add("User-Agent", "Albin-MicroServices"); // Optional but recommended
                client.Timeout = TimeSpan.FromSeconds(60);
            });

            //adding common DI services to Container.
            Services.AddSingletonServices().AddScopedServices().AddTransientServices();

            //adding WarmUpService to the DI.
            Services.AddHostedService<WarmUpService>();

            // Add Controllers to the container.
            Services.AddControllers();

            // Add Authentication and Authorization services
            if (configTemplate.IsServiceAuthorizationNeeded)
            {
                Services.AddAuthSetup();
            }
        }

        public static void UseKernelMiddlewares(this IApplicationBuilder app, IHost host, IEndpointRouteBuilder route, IWebHostEnvironment env, WebAppConfigs configs)
        {
            //Setting the Web App Mode.
            StaticMeths.SetGlobalWebAppMode(env.IsDevelopment(), env.IsStaging(), env.IsProduction());

            string AppName = env.ApplicationName[18..];

            // Configure the HTTP request pipeline.
            if ((env.IsDevelopment() || env.IsStaging()) && configs.IsSwaggerEnabled) // only show Swagger in Development and Staging [for Production Or Live using this ["Swagger:Enabled"]]
            {
                app.UseSwagger();
                app.UseSwaggerUI(options =>
                {
                    IApiVersionDescriptionProvider provider = host.Services.GetRequiredService<IApiVersionDescriptionProvider>();
                    for (int i = 0; i < provider.ApiVersionDescriptions.Count; i++)
                    {
                        ApiVersionDescription? description = provider.ApiVersionDescriptions[i];
                        options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", $"{AppName} API {description.GroupName}");
                    }
                    options.RoutePrefix = "swagger";
                });
            }

            // Redirect HTTP to HTTPS
            if (!env.IsDevelopment()) // Only force HTTPS in Staging, Production, etc..
            {
                app.UseHttpsRedirection();
            }
            else
            {
                app.UseDeveloperExceptionPage(); // Only in Development
            }

            // custom Middlewares registration
            if (configs.OnlyViaGateway)
            {
                app.UseMiddleware<RequireGatewayHeaderMiddleware>(); // Custom middleware to check for gateway header
            }

            app.UseSerilogRequestLogging(); // Enable Serilog request logging (Optional but recommended)

            //Auth Middlewares
            if (configs.IsServiceAuthorizationNeeded)
            {
                app.UseAuthentication();
                app.UseAuthorization();
            }

            route.MapControllers();

            //to serve static files like images, css, js, etc.
            app.UseStaticFiles();

            route.MapGet("/readiness", () => Results.Ok($"{AppName} Service is ready to Serve!!!"));

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
