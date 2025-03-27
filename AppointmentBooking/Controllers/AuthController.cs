using Application.Models;
using Application.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace AppointmentBooking.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAuthRepository _authRepository;
        public AuthController(IAuthRepository authRepository)
        {
            _authRepository = authRepository;
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterUser(UserDto registerUserDto)
        {
            var returnDto = await _authRepository.RegisterUser(registerUserDto);
            return Ok(returnDto);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginUserDto loginUserDto)
        {
            var result = await _authRepository.Login(loginUserDto);
            if (result is null)
            {
                return BadRequest();
            }
            return Ok(result);
        }
    }
}
