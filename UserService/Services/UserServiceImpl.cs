using FitnessTracker.Enums;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;
using UserService.DTOs;
using UserService.Models;
using UserService.Repositories;

namespace UserService.Services
{
    public class UserServiceImpl : IUserService
    {
        private readonly UserContext _userDbContext;

        public UserServiceImpl(UserContext userContext)
        {
            _userDbContext = userContext;
        }

        public async Task<UserDTO> Login(LoginRequestDTO loginRequestDTO)
        {
            if (loginRequestDTO.Username == null || loginRequestDTO.Username.Equals(""))
            {
                throw new InvalidDataException("Username is required");
            }

            if (loginRequestDTO.Password == null || loginRequestDTO.Password.Equals(""))
            {
                throw new InvalidDataException("Password is required");
            }

            loginRequestDTO.Password = GetSha256Hashed(loginRequestDTO.Password);

            User user = await _userDbContext.Users
                .Where(u => u.Username.Equals(loginRequestDTO.Username))
                .Where(u => u.Password.Equals(loginRequestDTO.Password))
                .Where(u => u.Status == CommonStatusEnum.ACTIVE)
                .FirstOrDefaultAsync();

            if (user == null)
            {
                throw new InvalidDataException("Invalid user credentials");
            }

            return EntityToDTO(user);
        }

        private static string GetSha256Hashed(string input)
        {
            var inputBytes = Encoding.UTF8.GetBytes(input);
            var inputHash = SHA256.HashData(inputBytes);
            return Convert.ToHexString(inputHash).ToLower();
        }

        private static UserDTO EntityToDTO(User user)
        {
            return new UserDTO()
            {
                Id = user.Id,
                Username = user.Username,
                Password = user.Password,
                FirstName = user.FirstName,
                FullName = user.FullName,
                DateOfBirth = user.DateOfBirth,
                Gender = user.Gender,
                MobileNumber = user.MobileNumber,
                Email = user.Email,
                Address = user.Address,
                Status = user.Status
            };
        }
    }
}
