using UserService.DTOs;

namespace UserService.Services
{
    public interface IUserService
    {
        Task<UserDTO> Login(LoginRequestDTO loginRequestDTO);
    }
}
