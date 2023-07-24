using FitnessTracker.Enums;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;
using System.Xml.Serialization;
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
                throw new InvalidDataException("Username is required");

            if (loginRequestDTO.Password == null || loginRequestDTO.Password.Equals(""))
                throw new InvalidDataException("Password is required");

            loginRequestDTO.Password = GetSha256Hashed(loginRequestDTO.Password);

            User user = await _userDbContext.Users
                .Where(u => u.Username.Equals(loginRequestDTO.Username))
                .Where(u => u.Password.Equals(loginRequestDTO.Password))
                .Where(u => u.Status == CommonStatusEnum.ACTIVE)
                .FirstOrDefaultAsync();

            if (user == null)
                throw new InvalidDataException("Invalid user credentials");

            return EntityToDTO(user);
        }

        public async Task<UserDTO> RegisterUser(UserDTO userDTO)
        {
            if (userDTO.Username == null || userDTO.Username.Equals(""))
                throw new InvalidDataException("Username is required");

            if (userDTO.Password == null || userDTO.Password.Equals(""))
                throw new InvalidDataException("Password is required");

            User user = await _userDbContext.Users
               .Where(u => u.Username.Equals(userDTO.Username))
               .Where(u => u.Status == CommonStatusEnum.ACTIVE)
               .FirstOrDefaultAsync();

            if (user != null)
                throw new InvalidDataException("Username is already taken");

            User newUser = DTOToEntity(userDTO);
            newUser.Status = CommonStatusEnum.ACTIVE;

            _userDbContext.Users.Add(newUser);
            await _userDbContext.SaveChangesAsync();

            return EntityToDTO(newUser);
        }

        public async Task<UserDTO> GetUserById(long id)
        {
            User user = await _userDbContext.Users
                .Where(u => u.Id.Equals(id))
                .Where(u => u.Status == CommonStatusEnum.ACTIVE)
                .FirstOrDefaultAsync();

            if (user == null) 
                throw new InvalidDataException("User id is invalid");

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

        private static User DTOToEntity(UserDTO userDTO)
        {
            return new User()
            {
                Username = userDTO.Username,
                Password = GetSha256Hashed(userDTO.Password),
                FirstName = userDTO.FirstName,
                FullName = userDTO.FullName,
                DateOfBirth = userDTO.DateOfBirth,
                Gender = userDTO.Gender,
                MobileNumber = userDTO.MobileNumber,
                Email = userDTO.Email,
                Address = userDTO.Address,
            };
        }

        private static string Serialize<T>(T dataToSerialize)
        {
            try
            {
                var stringwriter = new System.IO.StringWriter();
                var serializer = new XmlSerializer(typeof(T));
                serializer.Serialize(stringwriter, dataToSerialize);
                return stringwriter.ToString();
            }
            catch
            {
                throw;
            }
        }
    }
}
