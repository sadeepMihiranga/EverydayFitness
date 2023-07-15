using FitnessTracker.Enums;

namespace WorkoutService.DTOs
{
    public class BaseDTO
    {
        public long Id { get; set; }
        public CommonStatusEnum Status { get; set; }
    }
}
