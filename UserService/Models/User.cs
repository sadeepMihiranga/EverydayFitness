using UserService.Model;

namespace UserService.Models
{
    public class User : BaseModel
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string FullName { get; set; }
        public string Gender { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string? MobileNumber { get; set; }
        public string? Email { get; set; }
        public string? Address { get; set; }
    }
}
