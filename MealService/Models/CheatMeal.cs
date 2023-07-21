using FitnessTracker.Enums;
using MealService.Model;

namespace MealService.Models
{
    public class CheatMeal : BaseModel
    {
        public CheatMealType MealType { get; set; }
        public string Name { get; set; }
        public MealPortionSizeEnum MealPortionSize { get; set; }
        public CheatMealReason MealReason { get; set; }
        public CheatMealSatisfcationEnum CheatMealSatisfcation { get; set; }
        public int CaloriesTaken { get; set; }
        public DateTime DateTimeTaken { get; set; }
        public string Comment { get; set; }
        public int User { get; set; }
    }
}
