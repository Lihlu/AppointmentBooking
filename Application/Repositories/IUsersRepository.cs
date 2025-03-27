using Application.Models;
using Domain.Models.Entities;

namespace AppointmentBooking.Repositories
{
    public interface IUsersRepository
    {
        public Task<IEnumerable<User>> GetAllUsers();
        public Task<User?> GetUserById(int id);
        public Task<UserDto?> UpdateUser(int id, UserDto userDto);
        //public Task DeleteUser(int id);

    }
}
