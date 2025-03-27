using Application.Models;

namespace Application.Repositories
{
    public interface IAuthRepository
    {
        public Task<UserDto> RegisterUser(UserDto userDto);

        public Task<string?> Login(LoginUserDto loginUserDto);
    }
}
