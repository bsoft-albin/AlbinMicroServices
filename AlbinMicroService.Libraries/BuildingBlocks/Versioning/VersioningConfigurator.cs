using Asp.Versioning;
using Microsoft.Extensions.DependencyInjection;

namespace AlbinMicroService.Libraries.BuildingBlocks.Versioning
{
    public static class VersioningConfigurator
    {
        public static void ConfigureApiVersioning(this IServiceCollection services)
        {
            services.AddApiVersioning(options =>
            {
                options.DefaultApiVersion = new ApiVersion(1.0);
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.ReportApiVersions = true;
                options.Policies.Sunset(0.9)
                  .Effective(DateTimeOffset.Now.AddDays(60))
                  .Link("policy.html")
                  .Title("Versioning Policy")
                  .Type("text/html");
               // options.ApiVersionReader = new UrlSegmentApiVersionReader(); // URL segment versioning, if need some other types of versioning, add here...
            }).AddMvc().AddApiExplorer(options =>
            {
                options.GroupNameFormat = "'v'VVV";
                options.SubstituteApiVersionInUrl = true;
            });
        }
    }
}
