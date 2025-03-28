using AlbinMicroService.Users.Domain;
using Microsoft.Extensions.Options;
using Serilog;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Bind or Loading appsettings.json to the AppSettings class
builder.Services.Configure<AppSettings>(builder.Configuration);

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

// Enable Serilog request logging (Optional but recommended)
app.UseSerilogRequestLogging();

// Redirect HTTP to HTTPS
if (!app.Environment.IsDevelopment()) // Only force HTTPS in Staging and Production
{
    app.UseHttpsRedirection();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
