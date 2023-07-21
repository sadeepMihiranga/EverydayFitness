using Microsoft.AspNetCore.Mvc;
using UserService.DTOs;
using UserService.DTOs.Wrapper;
using UserService.Services;

namespace UserService.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class UserController : Controller
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost]
        public async Task<IActionResult> RegisterUser(UserDTO userDTO)
        {
            return Ok(new Response<UserDTO>(await _userService.RegisterUser(userDTO)));
        }

        [Route("login")]
        [HttpPost]
        public async Task<IActionResult> Login(LoginRequestDTO loginRequestDTO)
        {
            return Ok(new Response<UserDTO>(await _userService.Login(loginRequestDTO)));
        }

        [Route("{id}")]
        [HttpGet]
        public async Task<IActionResult> GetUserById(long id)
        {
            return Ok(new Response<UserDTO>(await _userService.GetUserById(id)));
        }
    }
}
