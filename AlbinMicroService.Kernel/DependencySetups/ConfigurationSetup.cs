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
                ApiVersion = configuration["Configs:ApiVersion"] ?? "v1",
                ApiTitle = configuration["Configs:ApiTitle"] ?? "AlbinMicroServices Api's",
                IsSwaggerEnabled = bool.Parse(configuration["Swagger:Enabled"]),
                IsThisGateway = bool.Parse(configuration["Configs:IsThisGateway"])
            };

            if (!webAppBuilderConfigTemplate.IsThisGateway)
            {
                webAppBuilderConfigTemplate.OnlyViaGateway = bool.Parse(configuration["Configs:OnlyViaGateway"]);
                webAppBuilderConfigTemplate.AuthorizeService = bool.Parse(configuration["Configs:AuthorizeService"]);
            }

#pragma warning restore CS8604 // Possible null reference argument.

            return webAppBuilderConfigTemplate;
        }
    }
}
