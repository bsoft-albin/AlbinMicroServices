using AlbinMicroService.Kernel.Concretes;
using AlbinMicroService.Kernel.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace AlbinMicroService.Kernel.DependencySetups
{
    public static class CommonDIServices
    {
        public static IServiceCollection AddTransientServices(this IServiceCollection services)
        {
            return services;
        }
        public static IServiceCollection AddScopedServices(this IServiceCollection services)
        {
            return services;
        }
        public static IServiceCollection AddSingletonServices(this IServiceCollection services)
        {
            services.AddSingleton<IKernelMeths, KernelMeths>();
            services.AddSingleton<IKernelProps, KernelProps>();

            return services;
        }
    }
}
