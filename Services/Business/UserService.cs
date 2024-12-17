using Core.Interfaces.Repositories;
using Core.Interfaces.Services;
using Core.Models;
using Microsoft.Extensions.Logging;

namespace Services.Business
{
    public class UserService: IUserService
    {
        private readonly IFoodRepository _foodRepository;
        private readonly ILogger<UserService> _logger;

        public UserService(IFoodRepository foodRepository, ILogger<UserService> logger)
        {
            _foodRepository = foodRepository;
            _logger = logger;
        }

        public async Task AddUserAsync(User user)
        {
            try
            {
                _logger.LogInformation("Добавление нового пользователя.");
                await _foodRepository.AddUserAsync(user);
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
            return await _foodRepository.LoadUsersAsync();
        }
    }
}