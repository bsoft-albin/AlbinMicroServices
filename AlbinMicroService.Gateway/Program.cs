WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Adding YARP Reverse Proxy
builder.Services.AddReverseProxy().LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

//Add logging to see how requests are routed <= checking the YaRp logs
if (builder.Environment.IsDevelopment()) {
    builder.Services.AddLogging(logging => {
        logging.AddConsole();
        logging.AddDebug();
    });
}

int HTTP_PORT = int.Parse(builder.Configuration["Configs:HttpPort"] ?? "9001");
int HTTPS_PORT = int.Parse(builder.Configuration["Configs:HttpsPort"] ?? "9002");
bool IsRunsInContainer = bool.Parse(builder.Configuration["Configs:IsRunningInContainer"] ?? "false");

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
    options.ListenAnyIP(HTTPS_PORT, listenOptions =>
    {
        listenOptions.UseHttps(); // Enable HTTPS
    });
});

WebApplication app = builder.Build();

// Redirect HTTP to HTTPS
if (!app.Environment.IsDevelopment()) // Only force HTTPS in Staging and Production
{
    app.UseHttpsRedirection();
}

// Enable endpoint routing
app.UseRouting();

app.MapReverseProxy(); // Enable YARP proxy

app.MapGet("/", () => "AlbinMicroServices Gateway Started Running Successfully....");

app.Run();
