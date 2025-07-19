using AlbinMicroService.Kernel.DependencySetups;
using AlbinMicroService.Libraries.BuildingBlocks.Versioning;
using AlbinMicroService.MasterData.Domain;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

WebAppConfigs configs = builder.AddDefaultServices();

builder.Services.ConfigureApiVersioning(configs);

//the below order must be crucial, as the services are dependent on each other.
builder.AddCustomServices().AddDatabaseServices().AddMasterDataServices();

WebApplication app = builder.Build();

app.UseKernelMiddlewares(app, app, app.Environment, configs); // Adding Kernel Middlewares to the container.
