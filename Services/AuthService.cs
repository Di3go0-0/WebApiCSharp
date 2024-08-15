using WebApi.Context;
using WebApi.Models;
using WebApi.DTOs;
using WebApi.Utils;
using Microsoft.EntityFrameworkCore;

namespace WebApi.Services
{
    public class AuthService
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly JWT _jwt;

        public AuthService(AppDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
            _jwt = new JWT(_configuration["Jwt:Key"]);
        }

        public async Task<string> RegisterAsync(RegisterUserDto registerDto)
        {
            if (await _context.Users.AnyAsync(u => u.Email == registerDto.Email))
            {
                return "Email already exists.";
            }

            var user = new User
            {
                Username = registerDto.Username,
                Email = registerDto.Email,
                Password = BCrypt.Net.BCrypt.HashPassword(registerDto.Password)
            };

            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            return "User registered successfully.";
        }

        public async Task<(string, string)> LoginAsync(LoginUserDto loginDto)
        {
            var user = await _context.Users.SingleOrDefaultAsync(u => u.Email == loginDto.Email);
            if (user == null || !BCrypt.Net.BCrypt.Verify(loginDto.Password, user.Password))
            {
                return (null, "Invalid email or password.");
            }

            var token = _jwt.GenerateToken(user.Email);
            return (token, null);
        }
    }
}
