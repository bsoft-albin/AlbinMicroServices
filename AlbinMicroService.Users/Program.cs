using AlbinMicroService.Kernel.DependencySetups;
using AlbinMicroService.Libraries.BuildingBlocks.Versioning;
using AlbinMicroService.Libraries.Common.QueryManager;
using AlbinMicroService.Users.Domain;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Read and bind AppSettings manually before 'Build()'
AppSettings appSettings = builder.Configuration.Get<AppSettings>() ?? new();

// Initialize Config with app settings
WebAppSettings.Initialize(appSettings);

WebAppConfigs configs = builder.AddDefaultServices();

builder.Services.ConfigureApiVersioning(configs);

builder.Services.AddSingleton(configs);

// we are Using Chain of Responsibility Pattern
builder.AddCustomServices().AddDatabaseServices().AddUserServices();

WebApplication app = builder.Build();

string SqlQueriesPath = Path.Combine(app.Environment.ContentRootPath, "Domain", "SqlQueries");
SqlQueryCache.Initialize(SqlQueriesPath);

StaticProps.SetGlobalWebAppSettings();

app.UseKernelMiddlewares(app, app, app.Environment, configs);
