using Infastructure.Data;
using Microsoft.AspNetCore.Mvc;
using AppointmentBooking.Repositories;
using Application.Models;
using Microsoft.AspNetCore.Authorization;

namespace AppointmentBooking.Controllers
{
    // localhost:xxxx/api/employees
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly ApplicationDbContext dbContext;
        private readonly IUsersRepository _usersRepository;

        public UsersController(ApplicationDbContext dbContext, IUsersRepository usersRepository)
        {
            this.dbContext = dbContext;
            _usersRepository = usersRepository;

        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            var allUsers = await _usersRepository.GetAllUsers();

            return Ok(allUsers);
        }

        [HttpGet]
        [Route("{id:int}")]
        public async Task<IActionResult> GetUserById(int id)
        {

            var user = await _usersRepository.GetUserById(id);

            if (user is null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        [HttpPut]
        [Route("{id:int}")]
        public async Task<IActionResult> UpdateUser(int id, UserDto updateUserDto)
        {
            UserDto? userDto = await _usersRepository.UpdateUser(id, updateUserDto);

            if (userDto is null)
            {
                return NotFound();
            }

            return Ok(userDto);
        }

        [HttpDelete]
        [Route("{id:int}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            string? result = await _usersRepository.DeleteUser(id);

            if (result is null)
            {
                return NotFound();
            }

            return Ok(result);
        }
    }
}
