using System.Text.Json;
using AlbinMicroService.Kernel.Loggings;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Provider.Polly;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

int HTTP_PORT = int.Parse(builder.Configuration["Configs:HttpPort"] ?? "9001");
int HTTPS_PORT = int.Parse(builder.Configuration["Configs:HttpsPort"] ?? "9002");
bool IsRunsInContainer = bool.Parse(builder.Configuration["Configs:IsRunningInContainer"] ?? "false");
bool IsHavingTLS = bool.Parse(builder.Configuration["Configs:IsHavingSSL"] ?? "false");

// 1. Create new config object to combine all route configs
string environment = builder.Environment.EnvironmentName;

string usersConfig = $"Ocelot/User/ocelot.{environment}.json";
string mastersConfig = $"Ocelot/Master/ocelot.{environment}.json";
string adminConfig = $"Ocelot/Admin/ocelot.{environment}.json";

// 2. Read and merge the route arrays
List<dynamic> allRoutes = [];
dynamic globalConfig = null!;

foreach (string file in new[] { usersConfig, mastersConfig, adminConfig })
{
    var json = File.ReadAllText(file);
    var config = JsonSerializer.Deserialize<JsonElement>(json);

    var routes = config.GetProperty("Routes").EnumerateArray();

    // Convert each JsonElement to dynamic and add to the list
    foreach (var route in routes)
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
builder.Configuration.AddJsonFile(mergedPath, optional: false, reloadOnChange: true);
builder.Services.AddOcelot().AddPolly().AddDelegatingHandler<RequestIdHandler>(true); // optional: tracking handler;

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

WebApplication app = builder.Build();

// Redirect HTTP to HTTPS
if (!app.Environment.IsDevelopment()) // Only force HTTPS in Staging and Production
{
    app.UseHttpsRedirection();
}

// Enable endpoint routing
app.UseRouting();

app.MapGet("/", () => "AlbinMicroServices Gateway Started Running Successfully....");

await app.UseOcelot();

app.Run();
