using AlbinMicroService.Core.Utilities;
using AlbinMicroService.Kernel.Middlewares;
using AlbinMicroService.Libraries.BuildingBlocks.Authentication;
using Asp.Versioning;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
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

            //adding common DI services to Container.
            Services.AddSingletonServices().AddScopedServices().AddTransientServices();

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

            // Add Authentication and Authorization services
            if (configTemplate.AuthorizeService) {
                Services.AddAuthSetup();
            }
        }

        public static void UseKernelMiddlewares(this IApplicationBuilder app, IHost host, IEndpointRouteBuilder route, IWebHostEnvironment env, WebAppBuilderConfigTemplate configs)
        {
            //Setting the Web App Mode.
            StaticMeths.SetGlobalWebAppMode(env.IsDevelopment(), env.IsStaging(), env.IsProduction());

            // Configure the HTTP request pipeline.

            if ((env.IsDevelopment() || env.IsStaging()) && configs.IsSwaggerEnabled) // only show Swagger in Development and Staging [for Production Or Live using this ["Swagger:Enabled"]]
            {
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", $"{env.ApplicationName[18..]} Api v1");
                    c.RoutePrefix = "swagger";
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
            if (configs.AuthorizeService) {
                app.UseAuthentication();
                app.UseAuthorization();
            }

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

        // not completed the below one...
        public static void AddVersioning(this IServiceCollection Services)
        {
            Services.AddApiVersioning(options =>
             {
                 options.DefaultApiVersion = new ApiVersion(1, 0);
                 options.AssumeDefaultVersionWhenUnspecified = true;
                 options.ReportApiVersions = true;
                 options.ApiVersionReader = ApiVersionReader.Combine(
                     new UrlSegmentApiVersionReader(),
                     new HeaderApiVersionReader("X-Api-Version")
                 );
             }).AddApiExplorer(options =>
             {
                 options.GroupNameFormat = "'v'VVV";
                 options.SubstituteApiVersionInUrl = true;
             });

            Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API - V1", Version = "v1.0" });
                c.SwaggerDoc("v2", new OpenApiInfo { Title = "My API - V2", Version = "v2.0" });
            });
        }
    }
}
