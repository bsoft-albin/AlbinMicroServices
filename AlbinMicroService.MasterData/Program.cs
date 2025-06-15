using AlbinMicroService.Kernel.DependencySetups;
using AlbinMicroService.MasterData.Domain;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

WebAppBuilderConfigTemplate configs = builder.AddDefaultServices();

builder.AddDatabaseServices().AddCustomServices().AddMasterDataServices();

WebApplication app = builder.Build();

app.UseKernelMiddlewares(app, app, app.Environment, configs); // Adding Kernel Middlewares to the container.
