using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ConsoleUI;
using Services.Business;
using Services.Utility;
using Services;
using DataBase;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Core.Interfaces.Repositories;
using Core.Interfaces.UI;
using Core.Interfaces.Services;

namespace FoodDiaryDb
{
    internal static class Program
    {
        static async Task Main(string[] args)
        {
            using var loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());
            ILogger<FoodDiaryManager> logger = loggerFactory.CreateLogger<FoodDiaryManager>();
            logger.LogInformation("Программа запущена.");

            // Создание хоста для DI
            var host = Host.CreateDefaultBuilder(args)
                .ConfigureServices((context, services) =>
                {
                    // Регистрация контекста базы данных с использованием SQLite
                    services.AddDbContext<FoodDbContext>(options =>
                        options.UseSqlite("Data Source=C:\\Users\\79217\\source\\repos\\FoodDiary\\DataBase\\foodDiary.db"));

                    services.AddSingleton<IFoodDbContextFactory>(provider =>
                    {
                        var options = provider.GetRequiredService<DbContextOptions<FoodDbContext>>();
                        return new DbContextFactory(options);
                    });

                    // Регистрация репозиториев и сервисов
                    // AddSingleton - Объект создается один раз за время жизни приложения
                    // АddScoped - объект хранит данные, которые должны быть уникальны для каждой операции --> не подходит
                    services.AddSingleton<IFoodRepository, FoodRepository>();
                    services.AddSingleton<IUserInputManager, UserInputManager>();
                    services.AddSingleton<ICalorieCalculator, CalorieCalculator>();
                    services.AddSingleton<IUserCreator, UserCreator>();
                    services.AddSingleton<IUserDataInitializer, UserDataInitializer>();
                    services.AddSingleton<IFoodService, FoodService>();
                    services.AddSingleton<IUserService, UserService>();
                    services.AddSingleton<IFoodManagementService, FoodManagementService>();
                    services.AddSingleton<ICalculateStatisticsService, CalculateStatisticsService>();
                    services.AddSingleton<IStatisticsService, StatisticsService>();
                    services.AddSingleton<IFoodDiaryManager, FoodDiaryManager>();
                    services.AddSingleton<IUserInterface, ConsoleUserInterface>();
                })
                .Build();



            // Получение экземпляра FoodDiaryManager
            var foodDiaryManager = host.Services.GetRequiredService<IFoodDiaryManager>();

            // Инициализация данных
            await foodDiaryManager.InitializerAsync(host.Services.GetRequiredService<IUserDataInitializer>());
        }
    }
}