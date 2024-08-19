using WebApi.DTOs;
using WebApi.Utils;
using WebApi.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace WebApi.Services
{
    public class AuthService
    {
        private readonly IConfiguration _configuration;
        private readonly JWT _jwt;
        private readonly Cookies _cookies;
        private readonly IAuthRepository _authRepository;

        public AuthService(IConfiguration configuration, IAuthRepository authRepository, Cookies cookies)
        {
            _configuration = configuration;
            var jwtKey = _configuration["Jwt:Key"] ?? throw new ArgumentNullException("Jwt:Key", "JWT key cannot be null");
            _jwt = new JWT(jwtKey);
            _authRepository = authRepository;
            _cookies = cookies;
        }

        public async Task<string> RegisterAsync(RegisterUserDto registerDto)
        {
            if (await _authRepository.GetUserAsync(registerDto.Email) != null)
            {
                return "Email already exists.";
            }

            string mensaje = await _authRepository.RegisterAsync(registerDto);
            if (mensaje.Contains("Database error") || mensaje.Contains("Unexpected error"))
            {
                return mensaje;
            }

            return "User registered successfully.";
        }

        public async Task<string> LoginAsync(LoginUserDto loginDto, HttpResponse response)
        {
            var user = await _authRepository.GetUserAsync(loginDto.Email);
            if (user == null)
            {
                return "User not found.";
            }

            var authenticatedUser = await _authRepository.LoginAsync(loginDto);
            if (authenticatedUser == null)
            {
                return "Invalid password.";
            }

            var token = _jwt.GenerateToken(authenticatedUser.Email);

            var success = _cookies.SetCookie("token", token, response);

            if (!success)
            {
                return "Failed to generate token.";
            }

            return "Logged in successfully.";
        }
    }
}
