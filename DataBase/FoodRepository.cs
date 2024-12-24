using Core.Interfaces.Repositories;
using Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DataBase
{
    public class FoodRepository : IFoodRepository
    {
        private readonly IFoodDbContextFactory _dbContextFactory;
        private readonly ILogger<FoodRepository> _logger;

        public FoodRepository(IFoodDbContextFactory dbContextFactory, ILogger<FoodRepository> logger)
        {
            _dbContextFactory = dbContextFactory;
            _logger = logger;
        }

        //  Добавление данных: AddFoodAsyn добавляет новую запись о продукте в базу данных
        // 1. Создает новый экземпляр контекста базы данных через фабрику
        // 2. Добавляет объект `food` в таблицу `Foods`
        // 3. Сохраняет изменения в базе данных SaveChangesAsync()
        public async Task AddFoodAsync(Food food)
        {
            try
            {
                using var context = _dbContextFactory.CreateDbContext();
                await context.Foods.AddAsync(food);
                await context.SaveChangesAsync();
                _logger.LogInformation("Продукт успешно добавлен.");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ошибка при добавлении продукта: {ex.Message}");
                throw;
            }
        }

        //Получение данных: GetAllFoodsAsync возвращает все записи из таблицы Foods с использованием ToListAsync()
        public async Task<List<Food>> GetAllFoodsAsync()
        {
            using var context = _dbContextFactory.CreateDbContext();
            return await context.Foods.ToListAsync();
        }

        //  Добавление данных: AddUserAsync добавляет новую запись о пользователе в базу данных
        // 1. Создает новый экземпляр контекста базы данных через фабрику
        // 2. Добавляет объект `user` в таблицу `Users`
        // 3. Сохраняет изменения в базе данных SaveChangesAsync()
        public async Task AddUserAsync(User user)
        {
            try
            {
                using var context = _dbContextFactory.CreateDbContext();
                await context.Users.AddAsync(user);
                await context.SaveChangesAsync();
                _logger.LogInformation("Пользователь успешно добавлен.");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ошибка при добавлении пользователя: {ex.Message}");
                throw;
            }
        }

        // Метод возвращает всех пользователей 
        public async Task<List<User>> LoadUsersAsync()
        {
            using var context = _dbContextFactory.CreateDbContext();
            return await context.Users.ToListAsync();
        }

        // Метод проверяет, существует ли пользователь с заданным именем
        public async Task<bool> UserExistsAsync(string userName)
        {
            using var context = _dbContextFactory.CreateDbContext();
            return await context.Users.AnyAsync(u => u.Name == userName);
        }
    }
}