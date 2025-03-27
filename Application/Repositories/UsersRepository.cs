using Application.Models;
using AutoMapper;
using Domain.Models.Entities;
using Infastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace AppointmentBooking.Repositories
{
    public class UsersRepository : IUsersRepository
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IMapper _mapper;
        public UsersRepository(ApplicationDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<IEnumerable<User>> GetAllUsers()
        {
            return await _dbContext.Users.AsNoTracking().Include(user => user.Appointments).ToListAsync();
        }

        public async Task<User?> GetUserById(int id)
        {
            return await _dbContext.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<UserDto?> UpdateUser(int id, UserDto userDto)
        {
            var user = await GetUserById(id);

            if (user == null)
            {
                return null;
            }

            _mapper.Map(userDto, user);

            _dbContext.Users.Update(user);
            await _dbContext.SaveChangesAsync();

            return userDto;
        }

        public async Task<string?> DeleteUser(int id)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == id);

            if (user is null)
            {
                return null;
            }

            _dbContext.Users.Remove(user);
            await _dbContext.SaveChangesAsync();

            return "deleted";
        }
    }
}
