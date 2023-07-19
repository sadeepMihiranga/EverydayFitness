using FitnessTracker.Enums;

namespace MealService.DTOs
{
    public class BaseDTO
    {
        public long Id { get; set; }
        public CommonStatusEnum Status { get; set; }
    }
}
