using Core.Interfaces.Repositories;
using Core.Interfaces.Services;
using Core.Interfaces.UI;
using Core.Models;
using Microsoft.Extensions.Logging;
using System.Diagnostics.CodeAnalysis;

namespace Services.Utility
{
    [ExcludeFromCodeCoverage]
    public class UserDataInitializer : IUserDataInitializer
    {
        private readonly IFoodRepository _foodRepository;
        private readonly IUserCreator _userCreator;
        private readonly ILogger<UserDataInitializer> _logger;
        private readonly IUserInterface _userInterface;

        public UserDataInitializer(IFoodRepository foodRepository, IUserCreator userCreator, ILogger<UserDataInitializer> logger, IUserInterface userInterface)
        {
            _foodRepository = foodRepository;
            _userCreator = userCreator;
            _logger = logger;
            _userInterface = userInterface;
        }
        public async Task<User> CreateNewUserAsync()
        {
            User newUser = await _userCreator.CreateNewUserAsync();
            return newUser;
        }
        public async Task<User> InitializeUserDataAsync()
        {
            _logger.LogInformation($"Инициализация данных.");
            List<User> users = await _foodRepository.LoadUsersAsync();
            if (users.Count == 0)
            {
                await _userInterface.WriteMessageAsync("Нет существующих пользователей. Пожалуйста, создайте нового пользователя.");
                _logger.LogWarning("Файл данных не найден. Инициализация нового пользователя.");
                User newUser = await _userCreator.CreateNewUserAsync();
                return newUser;
            }

            await _userInterface.WriteMessageAsync("Выберите пользователя:");
            for (int i = 0; i < users.Count; i++)
            {
                await _userInterface.WriteMessageAsync($"{i + 1}. {users[i].Name}");
            }
            await _userInterface.WriteMessageAsync("Ваш выбор:");

            string choice = await _userInterface.ReadInputAsync();
            if (int.TryParse(choice, out int index) && index > 0 && index <= users.Count)
            {
                return users[index - 1];
            }
            else
            {
                await _userInterface.WriteMessageAsync("Ошибка! Пожалуйста, введите корректный номер.");
                _logger.LogWarning("Неверный ввод. Повторный запрос.");
                return await InitializeUserDataAsync();
            }
        }
    }
}