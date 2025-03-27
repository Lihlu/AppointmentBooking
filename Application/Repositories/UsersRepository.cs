
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Application.Models;
using AutoMapper;
using Domain.Models.Entities;
using Infastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace AppointmentBooking.Repositories
{
    public class UsersRepository : IUsersRepository
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly JwtSettings _jwtSettings;
        public UsersRepository(ApplicationDbContext dbContext, IMapper mapper, IOptions<JwtSettings> jwtSettings)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _jwtSettings = jwtSettings.Value;
        }

        public async Task<IEnumerable<User>> GetAllUsers()
        {
            return await _dbContext.Users.AsNoTracking().Include(user => user.Appointments).ToListAsync();
        }

        public async Task<User?> GetUserById(int id)
        {
            return await _dbContext.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<UserDto> AddUser(UserDto userDto)
        {
            var user = _mapper.Map<User>(userDto);
            var hashedPassword = new PasswordHasher<User>().HashPassword(user, userDto.Password);

            // TODO: Revisit this 
            user.PasswordHash = hashedPassword;

            await _dbContext.Users.AddAsync(user);
            await _dbContext.SaveChangesAsync();
            return _mapper.Map<UserDto>(user);
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


        //public async Task<string?> DeleteUser(int id)
        //{
        //    var user = _dbContext.Users.Find(id);

        //    if (user is null)
        //    {
        //        return null;
        //    }

        //    _dbContext.Users.Remove
        //      await  _dbContext.Users.SaveChangesAsync();

        //    return "deleted";
        //}

        public async Task<string?> Login(LoginUserDto loginUserDto)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == loginUserDto.Email);

            if (user is null)
            {
                return null;
            }

            var passwordVerified = new PasswordHasher<User>().VerifyHashedPassword(user, user.PasswordHash, loginUserDto.Password) == PasswordVerificationResult.Success;

            if (passwordVerified)
            {
                var token = CreateToken(user);
                return token;
            }

            return null;
        }


        private string CreateToken(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Email)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Token!));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);

            var tokenDescriptor = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddDays(1),
                signingCredentials: creds
                );

            return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);

        }
    }
}
