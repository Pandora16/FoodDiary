using Core.Models;

namespace Services.Business
{
    public static class MealTimeExtensions
    {
        public static string ToLocalizedString(this MealTimes mealTime)
        {
            return mealTime switch
            {
                MealTimes.Breakfast => "завтрак",
                MealTimes.Lunch => "обед",
                MealTimes.Dinner => "ужин",
                _ => throw new ArgumentOutOfRangeException(nameof(mealTime), mealTime, null)
            };
        }
    }
}
