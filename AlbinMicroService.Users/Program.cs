using AlbinMicroService.Kernel.DependencySetups;
using AlbinMicroService.Users.Domain;
using Microsoft.Extensions.Options;

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

app.UseKernelMiddlewares(app, app, app.Environment);
