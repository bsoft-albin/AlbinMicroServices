using AlbinMicroService.Kernel.DependencySetups;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

WebAppBuilderConfigTemplate configs = ConfigurationSetup.BindSettings(builder.Configuration);

builder.AddKernelServices(builder.WebHost, configs); // Adding Kernel Services to the container.

builder.AddSeriloggings(builder.Host); // Adding Serilog to the container.

WebApplication app = builder.Build();

app.UseKernelMiddlewares(app, app, app.Environment); // Adding Kernel Middlewares to the container.
