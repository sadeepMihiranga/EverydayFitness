using FitnessTracker.Enums;

namespace UserService.Model
{
    public class BaseModel
    {
        public long Id { get; set; }
        public CommonStatusEnum Status { get; set; }
    }
}
