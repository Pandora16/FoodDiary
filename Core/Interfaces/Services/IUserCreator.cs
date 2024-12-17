using Core.Models;

namespace Core.Interfaces.Services
{
    public interface IUserCreator
    {
        Task<User> CreateNewUserAsync();
    }
}
