using AlbinMicroService.Users.Domain;
using Microsoft.Extensions.Options;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// we are Using Chain of Responsibility Pattern
builder.AddDefaultServices().AddDatabaseServices().AddCustomServices().AddUserServices();

// Bind or Loading appsettings.json to the AppSettings class
builder.Services.Configure<AppSettings>(builder.Configuration);

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

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
