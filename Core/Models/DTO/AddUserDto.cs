namespace Core.Models.DTOs
{
    public class AddUserDto
    {
        public required string Name { get; set; }
        public int Height { get; set; }
        public double Weight { get; set; }
        public int Age { get; set; }
        public string Gender { get; set; }
        public string ActivityLevel { get; set; }
        public double TargetCalories { get; set; }
    }
}
