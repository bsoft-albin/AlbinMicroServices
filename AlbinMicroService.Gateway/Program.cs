using AlbinMicroService.Core.Utilities;
using AlbinMicroService.Gateway;
using Ocelot.Middleware;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

WebAppBuilderConfigTemplate configs = builder.AddDefaultServices();

WebApplication app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    // Redirect HTTP to HTTPS
    if (configs.IsHavingSSL)
    {
        app.UseHttpsRedirection();// Only force HTTPS in Staging, Production, etc..
    }
}

if ((app.Environment.IsDevelopment() || app.Environment.IsStaging()) && configs.IsSwaggerEnabled)
{
    app.UseSwagger();
    app.UseSwaggerForOcelotUI();

    //app.UseSwaggerForOcelotUI(opt =>
    //{
    //    opt.PathToSwaggerGenerator = "/swagger/docs";
    //});

    //app.UseSwaggerForOcelotUI(opt =>
    //{
    //    opt.PathToSwaggerGenerator = "/swagger/docs";
    //    opt.RoutePrefix = "swagger"; // sets /swagger/index.html
    //});
}

// Enable endpoint routing
app.UseRouting();

await app.UseOcelot();

app.Run();
