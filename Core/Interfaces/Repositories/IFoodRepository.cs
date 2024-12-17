using Core.Models;

namespace Core.Interfaces.Repositories
{
    public interface IFoodRepository
    {
        Task AddFoodAsync(Food food);
        Task<List<Food>> GetAllFoodsAsync();
        Task<List<User>> LoadUsersAsync();
        Task AddUserAsync(User newUser);
        Task<bool> UserExistsAsync(string userName);
    }
}
