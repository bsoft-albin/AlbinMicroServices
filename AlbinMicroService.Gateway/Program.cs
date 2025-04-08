using AlbinMicroService.Gateway;
using Ocelot.Middleware;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.AddDefaultServices();

WebApplication app = builder.Build();

// Redirect HTTP to HTTPS
if (!app.Environment.IsDevelopment()) // Only force HTTPS in Staging and Production
{
    app.UseHttpsRedirection();
}
else
{
    app.UseDeveloperExceptionPage();
}

// Enable endpoint routing
app.UseRouting();

app.MapGet("/", () => "AlbinMicroServices Gateway Started Running Successfully....");

await app.UseOcelot();

app.Run();
