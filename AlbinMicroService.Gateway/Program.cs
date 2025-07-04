using AlbinMicroService.Core.Utilities;
using AlbinMicroService.Gateway;
using Ocelot.Middleware;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

WebAppBuilderConfigTemplate configs = builder.AddDefaultServices();

WebApplication app = builder.Build();

//Http Pipieline Starts Here...
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

app.UseResponseCompression(); // adding Response Compression Middleware to Pipeline.

await app.UseOcelot();

await app.RunAsync(); //why not app.Run(0, ==> because in previous line, awaitbale operation called !!
//Http Pipeline Ends Here ...