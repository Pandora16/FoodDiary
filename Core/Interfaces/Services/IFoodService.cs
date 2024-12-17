using Core.Models;

namespace Core.Interfaces.Services
{
    public interface IFoodService
    {
        Task AddFoodAsync(Food food);
        Task<List<Food>> GetAllFoodsAsync();
    }
}
