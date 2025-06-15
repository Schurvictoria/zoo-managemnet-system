using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Application.Services;
using Application.Interfaces;
using Domain.Interfaces;
using Infrastructure.Repositories;
using Application.Configuration;

namespace ZooManagementSystem
{
    class Program
    {
        static async Task Main(string[] args)
        {
            try
            {
                var services = new ServiceCollection();
                ConfigureServices(services);
                
                var serviceProvider = services.BuildServiceProvider();
                
                // Инициализация логгера
                var logger = serviceProvider.GetRequiredService<ILoggingService>();
                await logger.LogInfoAsync("Zoo Management System started!");

                // Валидация сервисов
                var transferService = serviceProvider.GetRequiredService<IAnimalTransferService>();
                if (transferService is IBaseService baseService)
                {
                    var isValid = await baseService.ValidateAsync();
                    if (!isValid)
                    {
                        await logger.LogErrorAsync("Service validation failed");
                        return;
                    }
                }

                // Здесь можно добавить запуск основных сервисов
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Critical error: {ex.Message}");
            }
        }

        private static void ConfigureServices(IServiceCollection services)
        {
            // Конфигурация
            var appSettings = new AppSettings
            {
                Database = new DatabaseSettings
                {
                    ConnectionString = "your_connection_string_here"
                },
                Logging = new LoggingSettings
                {
                    LogLevel = "Info",
                    LogFilePath = "logs/app.log"
                }
            };
            services.AddSingleton(appSettings);

            // Регистрация репозиториев
            services.AddScoped<IAnimalRepository, AnimalRepository>();
            services.AddScoped<IEnclosureRepository, EnclosureRepository>();
            services.AddScoped<IFeedingScheduleRepository, FeedingScheduleRepository>();

            // Регистрация сервисов
            services.AddScoped<ILoggingService, LoggingService>();
            services.AddScoped<IAnimalTransferService, AnimalTransferService>();
            services.AddScoped<IFeedingOrganizationService, FeedingOrganizationService>();
            services.AddScoped<IZooStatisticsService, ZooStatisticsService>();
        }
    }
}
