using AlbinMicroService.Core.Utilities;
using AlbinMicroService.Gateway;
using Ocelot.Middleware;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

WebAppBuilderConfigTemplate configs = builder.AddDefaultServices();

WebApplication app = builder.Build();

// Redirect HTTP to HTTPS
if (!app.Environment.IsDevelopment() && configs.IsHavingSSL) // Only force HTTPS in Staging and Production
{
    app.UseHttpsRedirection();
}
else
{
    app.UseDeveloperExceptionPage();
}

// Enable endpoint routing
app.UseRouting();

await app.UseOcelot();

app.Run();
