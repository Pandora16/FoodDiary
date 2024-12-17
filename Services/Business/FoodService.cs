using Microsoft.Extensions.Logging;
using Core.Interfaces.Services;
using Core.Models;
using Core.Interfaces.Repositories;

namespace Services.Business
{
    public class FoodService : IFoodService
    {
        private readonly IFoodRepository _foodRepository;
        private readonly ILogger<FoodService> _logger;

        public FoodService(IFoodRepository foodRepository, ILogger<FoodService> logger)
        {
            _foodRepository = foodRepository;
            _logger = logger;
        }

        public async Task AddFoodAsync(Food food)
        {
            try
            {
                _logger.LogInformation("Добавление нового продукта.");

                bool userExists = await _foodRepository.UserExistsAsync(food.UserName);
                if (!userExists)
                {
                    _logger.LogWarning($"Пользователь с именем {food.UserName} не существует.");
                    throw new Exception($"Пользователь с именем {food.UserName} не существует.");
                }

                await _foodRepository.AddFoodAsync(food);
                _logger.LogInformation("Продукт успешно добавлен.");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ошибка при добавлении продукта: {ex.Message}");
                throw;
            }
        }

        public async Task<List<Food>> GetAllFoodsAsync()
        {
            return await _foodRepository.GetAllFoodsAsync();
        }
    }
}
