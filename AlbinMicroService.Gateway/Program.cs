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

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

if ((app.Environment.IsDevelopment() || app.Environment.IsStaging()) && configs.IsSwaggerEnabled)
{
    app.UseSwagger();
    app.UseSwaggerForOcelotUI();
}

// Enable endpoint routing
app.UseRouting();

await app.UseOcelot();

app.Run();
