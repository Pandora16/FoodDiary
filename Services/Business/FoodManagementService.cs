using Core.Interfaces.Repositories;
using Core.Interfaces.Services;
using Core.Interfaces.UI;
using Core.Models;

namespace Services.Business
{
    public class FoodManagementService : IFoodManagementService
    {
        private readonly IUserInputManager _inputManager;
        private readonly IUserInterface _userInterface;
        private readonly IFoodService _foodService;

        public FoodManagementService(IFoodService foodService, IUserInputManager inputManager, IUserInterface userInterface)
        {
            _foodService = foodService;
            _inputManager = inputManager;
            _userInterface = userInterface;
        }

        public async Task AddFoodAsync(User user)
        {
            Food food; 

            // Сбор данных о продукте от пользователя
            while (true)
            {
                await _userInterface.WriteMessageAsync("Введите название продукта: ");
                food = new Food()
                {
                    Name = await _userInterface.ReadInputAsync()
                };

                if (string.IsNullOrWhiteSpace(food.Name))
                {
                    await _userInterface.WriteMessageAsync("Ошибка! Название продукта не может быть пустым.");
                }
                else if (!food.Name.Any(char.IsLetter))
                {
                    await _userInterface.WriteMessageAsync("Ошибка! Название продукта не должно состоять только из цифр.");
                }
                else
                {
                    break;
                }
            }
            food.UserName = user.Name;
            // Сбор информации о питательной ценности продукта
            food.Calories = await _inputManager.GetPositiveDoubleAsync("Введите калорийность продукта (в ккал): ");
            food.Proteins = await _inputManager.GetPositiveDoubleAsync("Введите количество белков (в г): ");
            food.Fats = await _inputManager.GetPositiveDoubleAsync("Введите количество жиров (в г): ");
            food.Carbohydrates = await _inputManager.GetPositiveDoubleAsync("Введите количество углеводов (в г): ");
            food.MealTime = await _inputManager.GetMealTimeAsync();
            food.Date = DateTime.Now;

            // Передаем сохранение продукта в репозиторий
            await _foodService.AddFoodAsync(food);
            await _userInterface.WriteMessageAsync("Продукт успешно добавлен!");
        }
    }
}
