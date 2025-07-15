using AlbinMicroService.Kernel.DependencySetups;
using AlbinMicroService.MasterData.Domain;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

WebAppConfigs configs = builder.AddDefaultServices();

builder.AddDatabaseServices().AddCustomServices().AddMasterDataServices();

WebApplication app = builder.Build();

app.UseKernelMiddlewares(app, app, app.Environment, configs); // Adding Kernel Middlewares to the container.
