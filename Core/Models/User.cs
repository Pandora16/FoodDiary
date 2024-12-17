using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Core.Models
{
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Name { get; set; }
        public int Height { get; set; } // Рост в см
        public double Weight { get; set; } // Вес в кг
        public int Age { get; set; } // Возраст
        public string Gender { get; set; } // Пол ("м" или "ж")
        public string ActivityLevel { get; set; } // Уровень активности
        public double BMR { get; set; } // Основной обмен веществ
        public double TargetCalories { get; set; } // Целевая калорийность (например, для похудения)
    }
}
