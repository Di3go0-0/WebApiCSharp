using Microsoft.AspNetCore.Mvc;
using WebApi.Models;
using WebApi.Services;
using WebApi.DTOs;
using WebApi.Utils;

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

        [HttpPost("register")]
        public IActionResult Register([FromBody] RegisterUserDto registerDto)
        {
            if (registerDto == null)
            {
                return BadRequest("Invalid user data.");
            }

            var result = _authService.Register(registerDto);
            if (result == "Email already exists.")
            {
                return BadRequest(result);
            }

            return Ok(new { Message = result });
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginUserDto loginDto)
        {
            if (string.IsNullOrEmpty(loginDto.Email) || string.IsNullOrEmpty(loginDto.Password))
            {
                return BadRequest("Invalid login data.");
            }

            var (token, error) = _authService.Login(loginDto);
            if (error != null)
            {
                return Unauthorized(error);
            }

            var success = _cookies.SetCookie("token", token, Response);
            if (!success)
            {
                return StatusCode(500, "Failed to set cookie.");
            }

            return Ok(new { Token = token });
        }

        [HttpPost("logout")]
        public IActionResult Logout()
        {
            var success = _cookies.SetCookie("token", "", Response, DateTime.UtcNow.AddHours(-1));
            if (!success)
            {
                return StatusCode(500, "Failed to clear cookie.");
            }

            return Ok(new { Message = "Logged out successfully." });
        }
    }
}