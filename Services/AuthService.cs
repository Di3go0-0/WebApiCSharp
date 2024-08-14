using WebApi.Context;
using WebApi.Models;
using WebApi.DTOs;
using WebApi.Utils;

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

        public string Register(RegisterUserDto registerDto)
        {
            if (_context.Users.Any(u => u.Email == registerDto.Email))
            {
                return "Email already exists.";
            }

            var user = new User
            {
                Username = registerDto.Username,
                Email = registerDto.Email,
                Password = BCrypt.Net.BCrypt.HashPassword(registerDto.Password)
            };

            _context.Users.Add(user);
            _context.SaveChanges();
            return "User registered successfully.";
        }

        public (string, string) Login(LoginUserDto loginDto)
        {
            var user = _context.Users.SingleOrDefault(u => u.Email == loginDto.Email);
            if (user == null || !BCrypt.Net.BCrypt.Verify(loginDto.Password, user.Password))
            {
                return (null, "Invalid email or password.");
            }

            var token = _jwt.GenerateToken(user.Email);
            return (token, null);
        }
    }
}
