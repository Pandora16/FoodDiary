using Core.Models;

namespace Core.Interfaces.Services
{
    public interface ICalorieCalculator
    {
        double CalculateBMR(User user);
        double CalculateTotalCalories(User user);
    }
}
