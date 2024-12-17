using Core.Interfaces.Repositories;
using Core.Interfaces.Services;
using Core.Models;
namespace Services.Business
{
    public class CalculateStatisticsService: ICalculateStatisticsService
    {
        private readonly ICalorieCalculator _calorieCalculator;
        private readonly IFoodRepository _foodRepository;
        public CalculateStatisticsService(ICalorieCalculator calorieCalculator, IFoodRepository foodRepository) 
        {
            _calorieCalculator = calorieCalculator;
            _foodRepository = foodRepository;
        }
        public async Task<string> CalculateStatistic(User user, string choice)
        {
            string result = "";
            DateTime now = DateTime.Now;
            DateTime startDate = choice switch
            {
                "1" => now.Date,
                "2" => now.Date.AddDays(-7),
                "3" => now.Date.AddMonths(-1),
                _ => now.Date
            };
            var foods = await _foodRepository.GetAllFoodsAsync();
            var foodsInPeriod = foods.Where(f => f.Date >= startDate && f.Date <= now && f.UserName == user.Name).ToList();

            if (!foodsInPeriod.Any())
            {
                return "Нет данных для выбранного периода.\n";
            }

            var mealsGrouped = foodsInPeriod.GroupBy(f => f.MealTime)
                                            .ToDictionary(g => g.Key, g => g.ToList());

            double totalCaloriesConsumed = 0;

            result += "\nСтатистика потребленных калорий:\n";

            // Перебираем значения перечисления MealTime
            foreach (MealTimes meal in Enum.GetValues(typeof(MealTimes)))
            {
                if (mealsGrouped.TryGetValue(meal, out var foodsInMeal))
                {
                    string localizedMealName = meal.ToLocalizedString(); // Локализованное имя из расширения
                    result += $"\n{localizedMealName}:";

                    double mealCalories = 0;

                    foreach (var food in foodsInMeal)
                    {
                        result += $"{food.Name} - {food.Calories} ккал\n";
                        mealCalories += food.Calories;
                    }

                    totalCaloriesConsumed += mealCalories;
                    result += $"Всего на {localizedMealName}: {mealCalories} ккал\n";
                }
            }

            // Вычисляем BMR и общие сожженные калории за выбранный период
            user.BMR = _calorieCalculator.CalculateBMR(user); // Инициализация BMR
            double dailyCaloriesBurned = _calorieCalculator.CalculateTotalCalories(user);
            int daysInPeriod = (now - startDate).Days + 1; // Умножаем на количество дней, включая начальный и конечный день

            double totalCaloriesBurned = dailyCaloriesBurned * daysInPeriod; // Сожженные калории за весь период

            result += $"\nОбщая статистика за выбранный период времени:\n";
            result += $"Потреблено калорий: {totalCaloriesConsumed} ккал\n";
            result += $"Сожженные калории по расчету BMR: {totalCaloriesBurned} ккал\n";

            // Сравниваем потребленные калории с целевой калорийностью
            if (totalCaloriesConsumed <= totalCaloriesBurned && totalCaloriesConsumed <= user.TargetCalories)
            {
                result += "Поздравляем! Вы достигли ваших целевых показателей калорийности!\n";
            }
            else
            {
                result += "Целевая калорийность не достигнута. Не расстраивайтесь! Продолжайте стараться, и вы достигнете своей цели!\n";
            }
            return result;
        }
    }
}
