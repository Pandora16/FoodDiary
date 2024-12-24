using Core.Interfaces.Repositories;
using Core.Models;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace Data
{
    public class FoodRepository : IFoodRepository
    {
        private readonly string _filePath;
        private readonly ILogger<FoodRepository> _logger;

        public FoodRepository(string filePath, ILogger<FoodRepository> logger)
        {
            _filePath = filePath;
            _logger = logger;
        }

        private async Task SaveDataAsync(FoodDiaryData data)
        {
            try
            {
                string jsonData = JsonSerializer.Serialize(data, new JsonSerializerOptions { WriteIndented = true });
                await File.WriteAllTextAsync(_filePath, jsonData);
                _logger.LogInformation("Данные успешно сохранены.");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ошибка при сохранении данных: {ex.Message}");
                throw;
            }
        }

        public async Task<(List<User>, List<Food>)> LoadDataAsync()
        {
            try
            {
                if (!File.Exists(_filePath))
                {
                    _logger.LogWarning("Файл данных не найден. Создание нового файла.");
                    var initialData = new FoodDiaryData();
                    await SaveDataAsync(initialData); 
                    return (initialData.Users, initialData.Foods);
                }

                string jsonData = await File.ReadAllTextAsync(_filePath);
                var data = JsonSerializer.Deserialize<FoodDiaryData>(jsonData);

                if (data == null)
                {
                    _logger.LogWarning("Файл повреждён. Инициализация нового файла.");
                    var initialData = new FoodDiaryData();
                    await SaveDataAsync(initialData);
                    return (initialData.Users, initialData.Foods);
                }

                _logger.LogInformation("Данные пользователя и продукты успешно загружены.");
                return (data.Users, data.Foods);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ошибка при чтении файла: {ex.Message}");
                throw;
            }
        }

        public async Task AddFoodAsync(Food food)
        {
            try
            {
                var (users, foods) = await LoadDataAsync();
                food.Id = GenerateFoodId(foods); 
                foods.Add(food);
                await SaveDataAsync(new FoodDiaryData { Users = users, Foods = foods });
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
            var (_, foods) = await LoadDataAsync();
            return foods;
        }

        public async Task AddUserAsync(User user)
        {
            try
            {
                var (existingUser, foods) = await LoadDataAsync();
                user.Id = GenerateUserId(); 
                var users = await LoadUsersAsync();
                users.Add(user);
                await SaveDataAsync(new FoodDiaryData { Foods = foods, Users = users });
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
            var (users, foods) = await LoadDataAsync();
            return users;
        }

        // Проверка существования пользователя
        // 1) Загружает данные пользователей через LoadDataAsync
        // 2) Проверяет, существует ли пользователь с указанным именем
        public async Task<bool> UserExistsAsync(string userName)
        {
            try
            {
                var (users, _) = await LoadDataAsync();
                foreach (var user in users)
                {
                    // Методом сравнения .Equals проверяем, равны ли строки user.Name и userName, игнорируя регистр символов
                    if (user.Name.Equals(userName, StringComparison.OrdinalIgnoreCase))
                        return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ошибка при проверке существования пользователя: {ex.Message}");
                throw;
            }
        }

        // Метод генерирует ID пользователя
        // 1) Загружает всех пользователей (синхронно через .Result)
        // 2) Если пользователей нет, возвращает 1; иначе возвращает максимальный Id + 1
        private int GenerateUserId()
        {
            var users = LoadUsersAsync().Result;  
            if (users.Count == 0)
                return 1;
            else
                return users.Max(u => u.Id) + 1;
        }

        // Метод генерирует ID еды 
        // 1) Если список foods пустой, возвращает 1; иначе возвращает максимальный Id + 1
        private int GenerateFoodId(List<Food> foods)
        {
            if (foods.Count == 0)
                return 1;
            else
                return foods.Max(f => f.Id) + 1;
        }

        public class FoodDiaryData
        {
            public List<Food> Foods { get; set; } = new List<Food>();
            public List<User> Users { get; set; } = new List<User>();
        }
    }
}

