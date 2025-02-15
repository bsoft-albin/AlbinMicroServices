using AlbinMicroService.Core.Repository;

namespace AlbinMicroService.Users.Domain
{
    public static class UsersDependencyContainer
    {
        public static WebApplicationBuilder AddDefaultServices(this WebApplicationBuilder builder)
        {
            // Add services to the container.
            builder.Services.AddControllers();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            return builder;
        }
        public static WebApplicationBuilder AddDatabaseServices(this WebApplicationBuilder builder)
        {
            string connectionString = builder.Configuration.GetConnectionString(DatabaseTypes.SqlServer) ?? string.Empty;
            builder.Services.AddScoped<ISqlServerMapper>(sp => new SqlServerMapper(connectionString));

            return builder;
        }
        public static WebApplicationBuilder AddCustomServices(this WebApplicationBuilder builder)
        {
            //services.AddScoped<JWttoken>, IJWttoken>();

            return builder;
        }

        public static WebApplicationBuilder AddUserServices(this WebApplicationBuilder builder)
        {
            //services.AddScoped<IUsersService, UsersService>();

            return builder;
        }
    }
}
