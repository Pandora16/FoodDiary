using Microsoft.Extensions.Logging;
using ConsoleUI;
using Services.Business;
using Services.Utility;
using Services;
using Data;

namespace FoodDiary
{
    internal static class Program
    {
        static async Task Main(string[] args)
        {
            using var loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());
            ILogger logger = loggerFactory.CreateLogger("General");

            logger.LogInformation("Программа запущена.");

            // Ручное создание экземпляров классов
            var userInterface = new ConsoleUserInterface();
            var inputManager = new UserInputManager(userInterface);
            var calorieCalculator = new CalorieCalculator();
            var foodRepository = new FoodRepository("foodDiary.json", loggerFactory.CreateLogger<FoodRepository>());
            var foodService = new FoodService(foodRepository, loggerFactory.CreateLogger<FoodService>());
            var userService = new UserService(foodRepository, loggerFactory.CreateLogger<UserService>());
            var foodManagementService = new FoodManagementService(foodService, inputManager, userInterface);
            var userCreator = new UserCreator(inputManager, calorieCalculator, loggerFactory.CreateLogger<UserCreator>(), userService);
            var userDataInitializer = new UserDataInitializer(foodRepository, userCreator, loggerFactory.CreateLogger<UserDataInitializer>(), userInterface);
            var calculateStatisticsService = new CalculateStatisticsService(calorieCalculator, foodRepository);
            var statisticsService = new StatisticsService(userInterface, calculateStatisticsService);
            var foodDiaryManager = new FoodDiaryManager(foodManagementService, statisticsService, userInterface, loggerFactory.CreateLogger<FoodDiaryManager>(), userCreator);


            await foodDiaryManager.InitializerAsync(userDataInitializer);
        }
    }
}
