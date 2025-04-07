using Microsoft.Extensions.DependencyInjection;

namespace AlbinMicroService.Kernel.DependencySetups
{
    public static class ServiceDiscovery
    {
        public static IServiceCollection AddKernelServices(this IServiceCollection Services)
        {
            // Add Controllers to the container.
            Services.AddControllers();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            Services.AddEndpointsApiExplorer();
            Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new() { Title = "Users Api", Version = "v1" });
            });

            return Services;
        }
    }
}
