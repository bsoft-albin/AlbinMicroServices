WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Adding YARP Reverse Proxy
builder.Services.AddReverseProxy().LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

//// Add services to the container.
//builder.Services.AddControllers();
//// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
//builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();

int HTTP_PORT = int.Parse(builder.Configuration["Configs:HttpPort"] ?? "9001");
int HTTPS_PORT = int.Parse(builder.Configuration["Configs:HttpsPort"] ?? "9002");
bool IsRunsInContainer = bool.Parse(builder.Configuration["Configs:IsRunningInContainer"] ?? "false");

if (!builder.Environment.IsDevelopment()) // Apply redirection only in Staging/Prod
{
    builder.Services.AddHttpsRedirection(options =>
    {
        options.HttpsPort = HTTPS_PORT; // Redirect HTTP to HTTPS in non-dev environments
    });
}

// Configure Kestrel to listen on both HTTP and HTTPS
builder.WebHost.ConfigureKestrel(options =>
{
    options.AddServerHeader = false; // Removes "Server: Kestrel" from response headers
    options.ListenAnyIP(HTTP_PORT); // HTTP
    options.ListenAnyIP(HTTPS_PORT, listenOptions =>
    {
        listenOptions.UseHttps(); // Enable HTTPS
    });
});

WebApplication app = builder.Build();

// Redirect HTTP to HTTPS
if (!app.Environment.IsDevelopment()) // Only force HTTPS in Staging and Production
{
    app.UseHttpsRedirection();
}

app.UseRouting();

app.MapReverseProxy(); // Enable YARP proxy

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}

//app.UseHttpsRedirection();

//app.UseAuthorization();

//app.MapControllers();

app.Run();
