namespace Core.Models.DTOs
{
    public class AddFoodDto
    {
        public required string Name { get; set; }
        public required string UserName { get; set; }
        public double Calories { get; set; }
        public double Proteins { get; set; }
        public double Fats { get; set; }
        public double Carbohydrates { get; set; }
        public MealTimes MealTime { get; set; }
        public DateTime Date { get; set; }
    }
}
