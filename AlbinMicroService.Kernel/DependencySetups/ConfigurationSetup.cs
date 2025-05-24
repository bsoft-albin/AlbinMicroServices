using AlbinMicroService.Core.Utilities;
using Microsoft.Extensions.Configuration;

namespace AlbinMicroService.Kernel.DependencySetups
{
    public static class ConfigurationSetup
    {
        public static WebAppBuilderConfigTemplate BindSettings(IConfiguration configuration)
        {
#pragma warning disable CS8604 // Possible null reference argument.

            WebAppBuilderConfigTemplate webAppBuilderConfigTemplate = new()
            {
                HttpPort = int.Parse(configuration["Configs:HttpPort"]),
                HttpsPort = int.Parse(configuration["Configs:HttpsPort"]),
                IsHavingSSL = bool.Parse(configuration["Configs:IsHavingSSL"]),
                IsRunningInContainer = bool.Parse(configuration["Configs:IsRunningInContainer"]),
                IsThisGateway = bool.Parse(configuration["Configs:IsThisGateway"]),
                IsSwaggerEnabled = bool.Parse(configuration["Swagger:Enabled"])
            };

            if (!webAppBuilderConfigTemplate.IsThisGateway) // these configs, are not available in the Gateway Appsettings
            {
                webAppBuilderConfigTemplate.ApiVersion = configuration["Configs:ApiVersion"] ?? "v1";
                webAppBuilderConfigTemplate.ApiTitle = configuration["Configs:ApiTitle"] ?? "AlbinMicroServices Api's";
                webAppBuilderConfigTemplate.OnlyViaGateway = bool.Parse(configuration["Configs:OnlyViaGateway"]);
                webAppBuilderConfigTemplate.IsServiceAuthorizationNeeded = bool.Parse(configuration["Configs:IsServiceAuthorizationNeeded"]);
            }

#pragma warning restore CS8604 // Possible null reference argument.

            return webAppBuilderConfigTemplate;
        }
    }
}
