using System.Text.Json;
using AlbinMicroService.Kernel.Delegates;
using Ocelot.DependencyInjection;
using Ocelot.Provider.Polly;

namespace AlbinMicroService.Gateway.Ocelot
{
    public static class OcelotConfiguration
    {
        public static void AddOcelotConfigurations(this WebApplicationBuilder builder)
        {
            // 1. Create new config object to combine all route configs
            string environment = builder.Environment.EnvironmentName;

            string usersConfig = $"Ocelot/User/ocelot.{environment}.json";
            string mastersConfig = $"Ocelot/Master/ocelot.{environment}.json";
            string adminConfig = $"Ocelot/Admin/ocelot.{environment}.json";

            string swaggerOcelotConfig = $"Ocelot/ocelot.Swagger.json";

            // 2. Read and merge the route arrays
            List<dynamic> allRoutes = [];
            dynamic globalConfig = null!;

            foreach (string file in new[] { usersConfig, mastersConfig, adminConfig })
            {
                string json = File.ReadAllText(file);
                JsonElement config = JsonSerializer.Deserialize<JsonElement>(json);

                JsonElement.ArrayEnumerator routes = config.GetProperty("Routes").EnumerateArray();

                // Convert each JsonElement to dynamic and add to the list
                foreach (JsonElement route in routes)
                {
                    allRoutes.Add(route);
                }

                if (globalConfig is null && config.TryGetProperty("GlobalConfiguration", out var global))
                {
                    globalConfig = global;
                }
            }

            // 3. Build merged JSON dynamically
            var finalConfig = new
            {
                Routes = allRoutes,
                GlobalConfiguration = globalConfig
            };

            string finalJson = JsonSerializer.Serialize(finalConfig, new JsonSerializerOptions { WriteIndented = true });

            // 4. Save temporary merged config file
            string mergedPath = Path.Combine(AppContext.BaseDirectory, "ocelot.merged.json");
            File.WriteAllText(mergedPath, finalJson);

            // 5. Load Ocelot config from merged file
            builder.Configuration.AddJsonFile(mergedPath, optional: false, reloadOnChange: true).AddJsonFile(swaggerOcelotConfig, optional: false, reloadOnChange: true);
            builder.Services.AddOcelot().AddDelegatingHandler<GatewayHeaderHandler>(true).AddPolly().AddDelegatingHandler<RequestIdHandler>(true); // optional: tracking handler;
        }
    }
}
