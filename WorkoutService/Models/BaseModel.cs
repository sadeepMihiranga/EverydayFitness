using FitnessTracker.Enums;

namespace WorkoutService.Model
{
    public class BaseModel
    {
        public long Id { get; set; }
        public CommonStatusEnum Status { get; set; }
    }
}
