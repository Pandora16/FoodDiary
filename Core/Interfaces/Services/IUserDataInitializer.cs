using Core.Models;

namespace Core.Interfaces.Services
{
    public interface IUserDataInitializer
    {
        Task<User> CreateNewUserAsync();
        Task<User> InitializeUserDataAsync();
    }
}
