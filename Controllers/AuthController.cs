using Microsoft.AspNetCore.Mvc;
using WebApi.Services;
using WebApi.DTOs;
using WebApi.Utils;
using WebApi.Interfaces;
using WebApi.Models;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;
        private readonly Cookies _cookies;

        public AuthController(AuthService authService, Cookies cookies)
        {
            _authService = authService;
            _cookies = cookies;
        }

        // [HttpGet("{email}")]
        // public async Task<IActionResult> GetTaskByID(string email)
        // {
        //     User user = await _authRepository.GetUserAsync(email);
        //     return Ok(user);
        // }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterAsync([FromBody] RegisterUserDto registerDto)
        {

            var result = await _authService.RegisterAsync(registerDto);
            if (result == "Email already exists.")
            {
                return BadRequest(new { Message = result });
            }
            if (result.Contains("Database error") || result.Contains("Unexpected error"))
            {
                return StatusCode(500, new { Message = result });
            }

            return Ok(new { Message = result });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginUserDto loginDto)
        {
            var result = await _authService.LoginAsync(loginDto, Response);

            if (result == "Logged in successfully.")
            {
                return Ok(new { Message = result });
            }
            else if (result == "User not found." || result == "Invalid password.")
            {
                return Unauthorized(new { Message = result });
            }
            else if (result == "Failed to generate token.")
            {
                return StatusCode(500, new { Message = result });
            }

            return StatusCode(500, new { Message = "An unexpected error occurred." });
        }


        [HttpPost("logout")]
        public IActionResult Logout()
        {
            var success = _cookies.SetCookie("token", "", Response, DateTime.UtcNow.AddHours(-1));
            if (!success)
            {
                return StatusCode(500, new { Message = "Failed to clear cookie." });
            }

            return Ok(new { Message = "Logged out successfully." });
        }
    }
}
