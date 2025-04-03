WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "MasterData Api", Version = "v1" });
});

int HTTP_PORT = int.Parse(builder.Configuration["Configs:HttpPort"] ?? "8003");
int HTTPS_PORT = int.Parse(builder.Configuration["Configs:HttpsPort"] ?? "8004");
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

//Setting the Web App Mode.
StaticMeths.SetGlobalWebAppMode(app.Environment.IsDevelopment(), app.Environment.IsStaging(), app.Environment.IsProduction());

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment() || app.Environment.IsStaging()) // only show Swagger in Development and Staging
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "MasterData Api v1");
        c.RoutePrefix = "swagger"; // This is critical for YARP routing
    });
}

// Redirect HTTP to HTTPS
if (!app.Environment.IsDevelopment()) // Only force HTTPS in Staging and Production
{
    app.UseHttpsRedirection();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
