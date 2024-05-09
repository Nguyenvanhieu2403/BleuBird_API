using BlueBird.Respository.Configs;

namespace BlueBird.API.Startups
{
    public static class ConfigServices
    {
        public static void AddCustomService(this IServiceCollection services , IConfiguration configuration)
        {
            services.DependencyInjectionRepository(configuration);
        }
    }
}
