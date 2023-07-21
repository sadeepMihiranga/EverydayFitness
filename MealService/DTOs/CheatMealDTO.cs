using FitnessTracker.Enums;

namespace MealService.DTOs
{
    public class CheatMealDTO : BaseDTO
    {
        public CheatMealTypeDTO MealType { get; set; }
        public string Name { get; set; }
        public MealPortionSizeEnum MealPortionSize { get; set; }
        public CheatMealReasonDTO MealReason { get; set; }
        public CheatMealSatisfcationEnum CheatMealSatisfcation { get; set; }
        public int CaloriesTaken { get; set; }
        public DateTime DateTimeTaken { get; set; }
        public string Comment { get; set; }
        public int User { get; set; }
    }
}
