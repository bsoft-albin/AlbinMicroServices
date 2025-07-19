using AlbinMicroService.Core.Utilities;
using AlbinMicroService.Kernel.DependencySetups;
using AlbinMicroService.Libraries.BuildingBlocks.Versioning;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

WebAppConfigs configs = ConfigurationSetup.BindSettings(builder.Configuration);

builder.Services.ConfigureApiVersioning(configs);

builder.AddKernelServices(builder.WebHost, configs); // Adding Kernel Services to the container.

builder.AddSeriloggings(builder.Host); // Adding Serilog to the container.

WebApplication app = builder.Build();

app.UseKernelMiddlewares(app, app, app.Environment, configs);
