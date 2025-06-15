using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Domain.Interfaces;
using Infrastructure.Data;
using Infrastructure.Repositories;

namespace Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ZooDbContext>(options =>
            options.UseInMemoryDatabase("ZooDb"));

        services.AddSingleton<IAnimalRepository, AnimalRepository>();
        services.AddSingleton<IEnclosureRepository, EnclosureRepository>();
        services.AddSingleton<IFeedingScheduleRepository, FeedingScheduleRepository>();

        return services;
    }
} 