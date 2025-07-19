using AlbinMicroService.Core.Utilities;
using AlbinMicroService.Gateway;
using Ocelot.Middleware;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

WebAppConfigs configs = builder.AddDefaultServices();

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
    app.UseSwaggerForOcelotUI(options =>
    {
        options.PathToSwaggerGenerator = "/swagger/docs";
    });
}

// Enable endpoint routing
app.UseRouting();

app.UseStaticFiles();

app.UseResponseCompression(); // adding Response Compression Middleware to Pipeline.

app.UseCors("AllowFrontend"); // Enable CORS for Frontend applications

await app.UseOcelot();

await app.RunAsync(); //why not app.Run(0, ==> because in previous line, awaitbale operation called !!
//Http Pipeline Ends Here ...