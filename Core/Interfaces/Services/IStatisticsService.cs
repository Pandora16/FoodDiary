using Core.Models;

namespace Core.Interfaces.Services
{
    public interface IStatisticsService
    {
        Task ShowStatisticsAsync(User user);
    }
}
