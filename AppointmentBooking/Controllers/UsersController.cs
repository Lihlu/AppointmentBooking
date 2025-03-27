using Infastructure.Data;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Domain.Models.Entities;
using AppointmentBooking.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http.HttpResults;
using System.Threading.Tasks;
using Application.Models;

namespace AppointmentBooking.Controllers
{
    // localhost:xxxx/api/employees
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly ApplicationDbContext dbContext;
        private readonly IMapper _mapper;
        private readonly IUsersRepository _usersRepository;

        public UsersController(ApplicationDbContext dbContext, IMapper mapper, IUsersRepository usersRepository)
        {
            this.dbContext = dbContext;
            _mapper = mapper;
            _usersRepository = usersRepository;

        }
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
            var userDto = await _usersRepository.UpdateUser(id, updateUserDto);

            if (userDto is null)
            {
                return NotFound();
            }

            return Ok(userDto);
        }

        [HttpDelete]
        [Route("{id:int}")]
        public IActionResult DeleteUser(int id)
        {
            var user = dbContext.Users.Find(id);

            if (user is null)
            {
                return NotFound();
            }

            dbContext.Users.Remove(user);
            dbContext.SaveChanges();

            return Ok();
        }


    }
}
