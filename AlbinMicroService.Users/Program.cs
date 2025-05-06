using AlbinMicroService.Kernel.DependencySetups;
using AlbinMicroService.Users.Domain;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Read and bind AppSettings manually before 'Build()'
AppSettings appSettings = builder.Configuration.Get<AppSettings>() ?? new();

// Initialize Config with app settings
WebAppConfigs.Initialize(appSettings);

WebAppBuilderConfigTemplate configs = builder.AddDefaultServices();

// we are Using Chain of Responsibility Pattern
builder.AddDatabaseServices().AddCustomServices().AddUserServices();

WebApplication app = builder.Build();

StaticProps.SetGlobalWebAppSettings();

app.UseKernelMiddlewares(app, app, app.Environment, configs);
