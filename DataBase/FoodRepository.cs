using Core.Interfaces.Repositories;
using Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DataBase
{
    public class FoodRepository : IFoodRepository
    {
        private readonly FoodDbContext _context;
        private readonly ILogger<FoodRepository> _logger;

        public FoodRepository(FoodDbContext context, ILogger<FoodRepository> logger)
        {
            _context = context;
            _logger = logger;
        }
        private async Task<int> GenerateNewIdAsync<TEntity>() where TEntity : class
        {
            // Получаем максимальный ID для указанной сущности
            var maxId = await _context.Set<TEntity>().MaxAsync(e => (int?)typeof(TEntity).GetProperty("Id").GetValue(e)) ?? 0;
            return maxId + 1;
        }
        public async Task AddFoodAsync(Food food)
        {
            food.Id = await GenerateNewIdAsync<Food>();
            try
            {
                await _context.Foods.AddAsync(food);
                await _context.SaveChangesAsync();
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
            return await _context.Foods.ToListAsync();
        }

        public async Task AddUserAsync(User user)
        {
            user.Id = await GenerateNewIdAsync<User>();
            try
            {
                await _context.Users.AddAsync(user);
                await _context.SaveChangesAsync();
                _logger.LogInformation("Пользователь успешно добавлен.");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ошибка при добавлении пользователя: {ex.Message}");
                throw;
            }
        }

        public async Task<List<User>> LoadUsersAsync()
        {
            return await _context.Users.ToListAsync();
        }
        public async Task<bool> UserExistsAsync(string userName)
        {
            return await _context.Users.AnyAsync(u => u.Name == userName);
        }
    }

}
