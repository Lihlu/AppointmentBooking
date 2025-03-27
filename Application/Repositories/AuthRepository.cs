using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Application.Models;
using AutoMapper;
using Domain.Models.Entities;
using Infastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Application.Repositories
{
    public class AuthRepository : IAuthRepository
    {
        private readonly IMapper _mapper;
        private readonly ApplicationDbContext _dbContext;
        private readonly JwtSettings _jwtSettings;

        public AuthRepository(IMapper mapper, ApplicationDbContext dbContext, IOptions<JwtSettings> jwtSettings)
        {
            _mapper = mapper;
            _dbContext = dbContext;
            _jwtSettings = jwtSettings.Value;
        }

        public async Task<UserDto?> RegisterUser(UserDto userDto)
        {
            bool userExists = await _dbContext.Users.AnyAsync(u => u.Email == userDto.Email);

            if (userExists)
            {
                return null;
            }

            var user = _mapper.Map<User>(userDto);
            var hashedPassword = new PasswordHasher<User>().HashPassword(user, userDto.Password);

            // TODO: Revisit this 
            user.PasswordHash = hashedPassword;

            await _dbContext.Users.AddAsync(user);
            await _dbContext.SaveChangesAsync();
            return _mapper.Map<UserDto>(user);
        }
        public async Task<TokenResponseDto?> Login(LoginUserDto loginUserDto)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == loginUserDto.Email);

            if (user is null)
            {
                return null;
            }

            var passwordVerified = new PasswordHasher<User>().VerifyHashedPassword(user, user.PasswordHash, loginUserDto.Password) == PasswordVerificationResult.Success;

            if (!passwordVerified)
            {
                return null;
            }

            var response = new TokenResponseDto
            {
                AccessToken = CreateToken(user),
                RefreshToken = await GenerateAndSaveRefreshTokenAsync(user)
            };
            return response;
        }

        private string CreateToken(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Email),
                new Claim (ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Role, user.Role),
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

        private string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

        private async Task<string> GenerateAndSaveRefreshTokenAsync(User user)
        {
            var refreshToken = GenerateRefreshToken();
            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
            await _dbContext.SaveChangesAsync();
            return refreshToken;
        }
    }
}
