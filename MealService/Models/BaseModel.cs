using FitnessTracker.Enums;

namespace MealService.Model
{
    public class BaseModel
    {
        public long Id { get; set; }
        public CommonStatusEnum Status { get; set; }
    }
}
