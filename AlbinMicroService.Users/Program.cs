using AlbinMicroService.Users.Domain;
using Microsoft.Extensions.Options;
using Serilog;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Bind or Loading appsettings.json to the AppSettings class
builder.Services.Configure<AppSettings>(builder.Configuration);

// Define MySQL errorlog connection string
string logDBConnectionString = builder.Configuration.GetConnectionString("LogDbConnection") ?? string.Empty;

if (!string.IsNullOrEmpty(logDBConnectionString)) {
    // Configure Serilog
    Log.Logger = new LoggerConfiguration()
        .WriteTo.Console() // Logs to console
        .WriteTo.File("logs/log-.txt", rollingInterval: RollingInterval.Day) // Logs to a daily rolling text file
        .WriteTo.MySQL(logDBConnectionString, "errorlogs") // Log to MySQL
        .CreateLogger();
}

// we are Using Chain of Responsibility Pattern
builder.AddDefaultServices().AddDatabaseServices().AddCustomServices().AddUserServices();

WebApplication app = builder.Build();

// Retrieve the settings (read-only)
AppSettings appSettings = app.Services.GetRequiredService<IOptions<AppSettings>>().Value;

// Initialize Config with app settings
WebAppConfigs.Initialize(appSettings);

//Setting the Web App Mode.
StaticMeths.SetGlobalWebAppMode(app.Environment.IsDevelopment(), app.Environment.IsStaging(), app.Environment.IsProduction());

// Configure the HTTP request pipeline.
if ((app.Environment.IsDevelopment() || app.Environment.IsStaging()) && WebAppConfigs.Settings.Swagger.Enabled) // only show Swagger in Development and Staging [for Production Or Live using this [WebAppConfigs.Settings.Swagger.Enabled]]
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

if (!string.IsNullOrEmpty(logDBConnectionString))
{
    // Enable request logging
    app.UseSerilogRequestLogging();
} 

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
