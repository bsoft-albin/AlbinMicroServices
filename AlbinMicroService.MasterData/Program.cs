using AlbinMicroService.Kernel.DependencySetups;
using AlbinMicroService.MasterData.Domain;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

WebAppConfigs configs = builder.AddDefaultServices();

//the below order must be crucial, as the services are dependent on each other.
builder.AddCustomServices().AddDatabaseServices().AddMasterDataServices();

WebApplication app = builder.Build();

app.UseKernelMiddlewares(app, app, app.Environment, configs); // Adding Kernel Middlewares to the container.
