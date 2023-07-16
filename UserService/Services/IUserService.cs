using UserService.DTOs;

namespace UserService.Services
{
    public interface IUserService
    {
        Task<UserDTO> RegisterUser(UserDTO userDTO);

        Task<UserDTO> Login(LoginRequestDTO loginRequestDTO);

        Task<UserDTO> GetUserById(long id);
    }
}
