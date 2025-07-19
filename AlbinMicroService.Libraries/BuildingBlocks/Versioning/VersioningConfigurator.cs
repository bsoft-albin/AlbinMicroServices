using AlbinMicroService.Core.Utilities;
using Asp.Versioning;
using Asp.Versioning.ApiExplorer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace AlbinMicroService.Libraries.BuildingBlocks.Versioning
{
    public static class VersioningConfigurator
    {
        public static void ConfigureApiVersioning(this IServiceCollection services, WebAppConfigs configTemplate)
        {
            services.AddApiVersioning(options =>
            {
                options.DefaultApiVersion = new ApiVersion(1, 0);
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.ReportApiVersions = true;
                //options.Policies.Sunset(0.9)
                //  .Effective(DateTimeOffset.Now.AddDays(60))
                //  .Link("policy.html")
                //  .Title("Versioning Policy")
                //  .Type("text/html");  // ==> to indicate customers the deprecation policy of the API versions.
                options.ApiVersionReader = new UrlSegmentApiVersionReader(); // URL segment versioning, if need some other types of versioning, add here...
            }).AddMvc().AddApiExplorer(options =>
            {
                options.GroupNameFormat = "'v'VVV";
                options.SubstituteApiVersionInUrl = true;
            });

            services.AddEndpointsApiExplorer();

            services.AddSwaggerGen(options =>
            {
                var provider = services.BuildServiceProvider().GetRequiredService<IApiVersionDescriptionProvider>();

                for (int i = 0; i < provider.ApiVersionDescriptions.Count; i++)
                {
                    ApiVersionDescription? description = provider.ApiVersionDescriptions[i];
                    options.SwaggerDoc(description.GroupName, new OpenApiInfo()
                    {
                        Title = configTemplate.ApiTitle,
                        Version = description.GroupName
                    });
                }
            });

        }
    }
}
