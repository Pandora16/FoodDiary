using Core.Models;

namespace Core.Interfaces.Services
{
    public interface ICalculateStatisticsService
    {
        Task<string> CalculateStatistic(User user, string choice);
    }
}
