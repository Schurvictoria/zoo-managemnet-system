using Microsoft.Extensions.DependencyInjection;
using Application.Services;

namespace Application.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<AnimalService>();
            return services;
        }
    }
}
