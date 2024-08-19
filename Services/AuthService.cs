using WebApi.Context;
using WebApi.Models;
using WebApi.DTOs;
using WebApi.Utils;
using Microsoft.EntityFrameworkCore;
using WebApi.Interfaces;

namespace WebApi.Services
{
    public class AuthService
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly JWT _jwt;
        private readonly IAuthRepository _authRepository;

        public AuthService(AppDbContext context, IConfiguration configuration, IAuthRepository authRepository)
        {
            _context = context;
            _configuration = configuration;
            var jwtKey = _configuration["Jwt:Key"] ?? throw new ArgumentNullException("Jwt:Key", "JWT key cannot be null");
            _jwt = new JWT(jwtKey);
            _authRepository = authRepository;
        }

        public async Task<string> RegisterAsync(RegisterUserDto registerDto)
        {
            if (await _authRepository.GetUserAsync(registerDto.Email) != null)
            {
                return "Email already exists.";
            }

            string mensaje = await _authRepository.RegisterAsync(registerDto);
            return "User registered successfully.";
        }

        public async Task<(string?, string?)> LoginAsync(LoginUserDto loginDto)
        {
            var user = await _authRepository.LoginAsync(loginDto);
            if (user == null)
            {
                return (null, "Invalid email or password.");
            }

            var token = _jwt.GenerateToken(user.Email);
            return (token, null);
        }
    }
}
