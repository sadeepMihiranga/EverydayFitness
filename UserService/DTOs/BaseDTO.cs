using FitnessTracker.Enums;

namespace UserService.DTOs
{
    public class BaseDTO
    {
        public long Id { get; set; }
        public CommonStatusEnum Status { get; set; }
    }
}
