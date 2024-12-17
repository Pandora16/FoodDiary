using Core.Models;

namespace Core.Interfaces.Services
{
    public interface IUserService
    {
        Task AddUserAsync(User user);
        Task<List<User>> LoadUsersAsync();
    }
}
