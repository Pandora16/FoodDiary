using Core.Models;

namespace Services.Business
{
    public static class MealTimeExtensions
    {
        //  Метод расширения для перечисления MealTimes. Возвращает локализованную строку на основе значения mealTime
        public static string ToLocalizedString(this MealTimes mealTime)
        {
            if (mealTime == MealTimes.Breakfast)
                return "завтрак";
            else if (mealTime == MealTimes.Lunch)
                return "обед";
            else if (mealTime == MealTimes.Dinner)
                return "ужин";
            else
                // выбрасывает исключение, если значение mealTime находится вне допустимого диапазона
                throw new ArgumentOutOfRangeException(nameof(mealTime), mealTime, null);
        }
    }
}
