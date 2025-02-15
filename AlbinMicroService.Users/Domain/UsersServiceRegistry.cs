namespace AlbinMicroService.Users.Domain
{
    public static class UsersServiceRegistry
    {
        public static IServiceCollection AddUserServices(this IServiceCollection services)
        {
            //services.AddScoped<IUsersService, UsersService>();

            return services;
        }
    }
}
