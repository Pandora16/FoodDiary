using Core.Interfaces.Services;
using Core.Interfaces.UI;
using Core.Models;
using Microsoft.Extensions.Logging;

namespace Services
{
    public class FoodDiaryManager : IFoodDiaryManager
    {
        private readonly IFoodManagementService _foodManagementService;
        private readonly IStatisticsService _statisticsService;
        private readonly IUserInterface _userInterface;
        private readonly ILogger<FoodDiaryManager> _logger;
        private readonly IUserCreator _userCreator;

        public FoodDiaryManager(IFoodManagementService foodManagementService, IStatisticsService statisticsService, IUserInterface userInterface, ILogger<FoodDiaryManager> logger, IUserCreator userCreator)
        {
            _foodManagementService = foodManagementService;
            _statisticsService = statisticsService;
            _userInterface = userInterface;
            _logger = logger;
            _userCreator = userCreator;
        }

        public async Task InitializerAsync(IUserDataInitializer userDataInitializer)
        {
            while (true)
            {
                await _userInterface.WriteMessageAsync("\nВыберите действие:");
                await _userInterface.WriteMessageAsync("1. Создать нового пользователя");
                await _userInterface.WriteMessageAsync("2. Выбрать пользователя");
                await _userInterface.WriteMessageAsync("3. Выход");
                await _userInterface.WriteMessageAsync("Ваш выбор (1-3): ");

                string choice = await _userInterface.ReadInputAsync();
                try
                {
                    User user;
                    switch (choice)
                    {
                        case "1":
                            user = await userDataInitializer.CreateNewUserAsync();
                            await RunAsync(user);
                            break;
                        case "2":
                            user = await userDataInitializer.InitializeUserDataAsync();
                            await RunAsync(user);
                            break;
                        case "3":
                            await _userInterface.WriteMessageAsync("Завершение работы...");
                            return;
                        default:
                            await _userInterface.WriteMessageAsync("Ошибка! Пожалуйста, введите число от 1 до 3.");
                            break;
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Ошибка в программе: {ex.Message}");
                }
            }
        }

        public async Task RunAsync(User user)
        {
            while (true)
            {
                await _userInterface.WriteMessageAsync("\nВыберите действие:");
                await _userInterface.WriteMessageAsync("1. Добавить продукт");
                await _userInterface.WriteMessageAsync("2. Показать статистику");
                await _userInterface.WriteMessageAsync("3. Назад");
                await _userInterface.WriteMessageAsync("Ваш выбор (1-3): ");

                string choice = await _userInterface.ReadInputAsync();

                switch (choice)
                {
                    case "1":
                        await _foodManagementService.AddFoodAsync(user);
                        break;
                    case "2":
                        await _statisticsService.ShowStatisticsAsync(user);
                        break;
                    case "3":
                        await _userInterface.WriteMessageAsync("...");
                        return;
                    default:
                        await _userInterface.WriteMessageAsync("Ошибка! Пожалуйста, введите число от 1 до 3.");
                        break;
                }
            }
        }
    }
}
