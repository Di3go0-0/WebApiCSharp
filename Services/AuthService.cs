using WebApi.DTOs;
using WebApi.Utils;
using WebApi.Interfaces;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace WebApi.Services
{
    public class AuthService : IAuthService
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

            return await _authRepository.RegisterAsync(registerDto);
        }

        public async Task<string> LoginAsync(LoginUserDto loginDto, HttpResponse response)
        {
            var authenticatedUser = await _authRepository.LoginAsync(loginDto);
            if (authenticatedUser == null)
            {
                return "Invalid credentials.";
            }

            var token = _jwt.GenerateToken(authenticatedUser.Email);
            if (!_cookies.SetCookie("token", token, response))
            {
                return "Failed to generate token.";
            }

            return "Logged in successfully.";
        }

        public string Logout(HttpResponse response)
        {
            if (!_cookies.SetCookie("token", "", response, DateTime.UtcNow.AddHours(-1)))
            {
                return "Failed to remove token.";
            }

            return "Logged out successfully.";
        }
    }
}
