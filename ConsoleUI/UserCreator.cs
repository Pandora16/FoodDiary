using Core.Interfaces.Repositories;
using Core.Interfaces.Services;
using Core.Interfaces.UI;
using Core.Models;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Services
{
    [ExcludeFromCodeCoverage]
   public class UserCreator : IUserCreator
    {
        private readonly IUserInputManager _inputManager;
        private readonly ICalorieCalculator _calorieCalculator;
        private readonly ILogger<UserCreator> _logger;
        private readonly IUserService _userService; 

        public UserCreator(IUserInputManager inputManager, ICalorieCalculator calorieCalculator, ILogger<UserCreator> logger, IUserService userService)
        {
            _inputManager = inputManager;
            _calorieCalculator = calorieCalculator;
            _logger = logger;
            _userService = userService; 
        }

        public async Task<User> CreateNewUserAsync()
        {
            Console.WriteLine("Добро пожаловать в электронный дневник питания!");

            string name;
            do
            {
                name = await _inputManager.GetUserNameAsync("Введите ваше имя: ");
            } while (await UserExistsAsync(name));

            User user = new User
            {
                Name = name,
                Height = await _inputManager.GetPositiveIntegerAsync("Введите ваш рост (в см): "),
                Weight = await _inputManager.GetPositiveIntegerAsync("Введите ваш вес (в кг): "),
                Age = await _inputManager.GetPositiveIntegerAsync("Введите ваш возраст (в годах): "),
                Gender = await _inputManager.GetGenderAsync(),
                ActivityLevel = await _inputManager.GetActivityLevelAsync()
            };
            user.BMR = _calorieCalculator.CalculateBMR(user);
            user.TargetCalories = await _inputManager.GetPositiveIntegerAsync("Введите вашу целевую калорийность (в ккал): ");

            await _userService.AddUserAsync(user);

            _logger.LogInformation("Новый пользователь успешно создан.");
            return user;
        }

        private async Task<bool> UserExistsAsync(string name)
        {
            var users = await _userService.LoadUsersAsync();

            if (users == null)
            {
                return false; 
            }

            return users.Any(u => !string.IsNullOrEmpty(u.Name) && u.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
        }
    }
}
