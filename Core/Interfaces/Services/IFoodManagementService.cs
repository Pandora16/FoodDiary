using Core.Models;

namespace Core.Interfaces.Services
{
    public interface IFoodManagementService
    {
        Task AddFoodAsync(User user);
    }
}
